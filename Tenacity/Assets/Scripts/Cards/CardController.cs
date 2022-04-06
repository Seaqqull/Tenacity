using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tenacity.Cards
{
    public class CardController : MonoBehaviour
    {
        public static Card CreateCardCreatureOnBoard(Card newCard)
        {
            GameObject loadedCreatureObject = LoadFromDatabase(newCard);
            if (loadedCreatureObject == null) return null;

            var creatureGO = Instantiate(loadedCreatureObject) as GameObject;
            creatureGO.transform.SetParent(newCard.transform.parent);
            creatureGO.transform.localPosition = newCard.transform.localPosition;

            Card cardComponent = creatureGO.GetComponent<Card>();
            cardComponent.Data = newCard.Data;
            cardComponent.IsDraggable = newCard.IsDraggable;
            cardComponent.State = CardState.OnBoard;
            Destroy(newCard.gameObject);
            return cardComponent;
        }

        private static GameObject LoadFromDatabase(Card newCard)
        {
            return AssetDatabase.LoadAssetAtPath(
                $"Assets/StaticAssets/Prefabs/Creatures/{newCard.Data.Type}_{newCard.Data.CardId}.prefab", typeof(GameObject)
                ) as GameObject;
        }
    }
}
