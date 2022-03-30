using Tenacity.Cards;
using Tenacity.Lands;
using UnityEditor;
using UnityEngine;

namespace Tenacity.Draggable
{
    public class DraggableBattleCardController : DragAndDrop<Card>
    {
        [SerializeField] private CardDeckManager cardDeckManager;

        private Card SelectedCard => SelectedGO?.GetComponent<Card>();
        private bool _isTurnEnded;

        private string DroppedCardAssetPath
        {
            get => (SelectedCard != null && SelectedCard.Data != null)
                    ? $"Assets/StaticAssets/Prefabs/Creatures/{SelectedCard.Data.Type}_{SelectedCard.Data.CardId}.prefab"
                    : "";
        }

        public bool IsTurnEnded
        {
            get => _isTurnEnded;
            set => _isTurnEnded = value;
        }

        protected override bool DropSelectedObject(GameObject target)
        {
            if (target.GetComponent<Land>() == null) return false;
            
            Land land = target.GetComponent<Land>();

            if (!land.IsAvailableForCards || land.Type != SelectedCard.Data.Land)
            {
                GetBackSelectedObject();
                return false;
            }

            if (SelectedCard.transform.parent != null && SelectedCard.transform.parent.TryGetComponent<Land>(out Land prevLand)) 
                prevLand.IsAvailableForCards = true;
            land.IsAvailableForCards = false;

            if (SelectedCard.State != CardState.OnBoard) 
                CreateCardOnBoard(SelectedCard, land);
            else 
                PlaceSelectedObject(target);

            _isTurnEnded = true;
            return true;
        }

        protected override bool IsDraggable(GameObject gameObject)
        {
            if (!gameObject.GetComponent<Card>()) return false;
            return gameObject.GetComponent<Card>().IsDraggable;
        }

        private void CreateCardOnBoard(Card card, Land land)
        {
            Object cardCreaturePref = AssetDatabase.LoadAssetAtPath(DroppedCardAssetPath, typeof(GameObject));
            if (cardCreaturePref == null) return;

            GameObject cardCreatureGO = Instantiate(cardCreaturePref) as GameObject;
            cardCreatureGO.transform.parent = land.transform;
            cardCreatureGO.transform.localPosition = new Vector3(0, DroppedObjectYPos, 0);

            Card cardComponent;
            if (!cardCreatureGO.TryGetComponent<Card>(out cardComponent))
            {
                Destroy(cardCreatureGO.gameObject);
                GetBackSelectedObject();
                return;
            }
            cardComponent.Data = card.Data;
            cardComponent.IsDraggable = card.IsDraggable;
            cardComponent.State = CardState.OnBoard;

            cardDeckManager.ReplaceCard(card, cardComponent);
            Destroy(card.gameObject);
        }

    }
}