using Tenacity.Battles.Lands;
using Tenacity.Cards.Data;
using UnityEngine;


namespace Tenacity.Cards.Managers
{
    public class CardManager : MonoBehaviour
    {
        public static Card CreateCardCreatureOnBoard(Card newCard, Land landToPlace)
        {
            GameObject loadedCreatureObject = LoadFromDatabase(newCard);
            if (loadedCreatureObject == null) return null;

            var creatureGO = Instantiate(loadedCreatureObject, landToPlace.transform);
            creatureGO.transform.localPosition = new Vector3(0, landToPlace.TopPoint, 0);

            var cardComponent = creatureGO.GetComponent<Card>();
            cardComponent.State = CardState.OnBoard;
            cardComponent.Data = newCard.Data;
            
            Destroy(newCard.gameObject);
            return cardComponent;
        }

        private static GameObject LoadFromDatabase(Card newCard)
        {
            return Resources.Load<GameObject>($"Creatures/{newCard.Data.Type}_{newCard.Data.CardId}");
        }
    }
}
