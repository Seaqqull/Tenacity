using Tenacity.Battle;
using Tenacity.Cards;
using Tenacity.Lands;
using UnityEngine;

namespace Tenacity.Draggable
{
    public class DraggableCardController : DragAndDrop<Card>
    {
        [SerializeField] private BattleController _battle;

        private Card SelectedCard => SelectedGO?.GetComponent<Card>();


        protected override void OnEndDragging()
        {
            var parentLand = SelectedCard.transform.parent?.GetComponent<LandController>();
            if (parentLand != null) parentLand.HighlightNeighbors(SelectedCard.Data.Land, false);

            base.OnEndDragging();
        }

        protected override bool DropSelectedObject(GameObject target)
        {
            if (target.GetComponent<Land>() == null) return false;

            Land targetLand = target.GetComponent<Land>();
            var parentLand = SelectedCard.transform.parent?.GetComponent<Land>();

            if ( (SelectedCard.Place != null) && (!SelectedCard.Place.IsNeighborListContains(targetLand)) )
            {
                GetBackSelectedObject();
                return false;
            }
            if (SelectedCard.State == CardState.OnBoard && targetLand.GetPlacedCreature())
            {
                Card targetCreature = targetLand.GetPlacedCreature();
                if (_battle.Player.GetCreaturesToAttack(SelectedCard).Contains(targetCreature))
                {
                    _battle.Player.Attack(SelectedCard, targetCreature);
                    SelectedCard.IsDraggable = false;
                    GetBackSelectedObject();
                    return true;
                }
                GetBackSelectedObject();
                return false;

            }
            if (!targetLand.IsAvailableForCards || !targetLand.Type.HasFlag(SelectedCard.Data.Land))
            {
                GetBackSelectedObject();
                return false;
            }

            if (SelectedCard.transform.parent != null
                && SelectedCard.transform.parent.TryGetComponent<Land>(out Land prevLand))
            {
                prevLand.IsAvailableForCards = true;
            }
            targetLand.IsAvailableForCards = false;

            if (SelectedCard.State == CardState.OnBoard)
            {
                PlaceSelectedObject(target);
            }
            else
            {
                SelectedCard.transform.parent = targetLand.transform;
                SelectedCard.transform.localPosition = new Vector3(0, DroppedObjectYPos, 0);
                _battle.Player.RemoveCard(SelectedCard);
                Card creature = CardController.CreateCardCreatureOnBoard(SelectedCard);
                _battle.Player.AddNewCard(creature);
            }

            SelectedCard.IsDraggable = false;
            return true;
        }

        protected override bool IsDraggable(GameObject gameObject)
        {
            return gameObject.GetComponent<Card>() && gameObject.GetComponent<Card>().IsDraggable;
        }

        protected override void OnStartDragging(GameObject clickedObject)
        {
            base.OnStartDragging(clickedObject);
            if (_battle.Player.CurrentMana < clickedObject.GetComponent<Card>().Data.CastingCost)
            {
                GetBackSelectedObject();
                return;
            }
            var parentLand = clickedObject.transform.parent?.GetComponent<LandController>();
            if (parentLand != null) parentLand.HighlightNeighbors(SelectedCard.Data.Land, true);
        }
    }
}