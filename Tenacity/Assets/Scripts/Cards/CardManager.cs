using System.Collections;
using System.Collections.Generic;
using Tenacity.Lands;
using UnityEditor;
using UnityEngine;

namespace Tenacity.Cards
{
    public class CardManager : MonoBehaviour
    {
        public static Card CreateCardCreatureOnBoard(Card newCard, Land landToPlace)
        {
            GameObject loadedCreatureObject = LoadFromDatabase(newCard);
            if (loadedCreatureObject == null) return null;

            var creatureGO = Instantiate(loadedCreatureObject, landToPlace.transform);
            creatureGO.transform.localPosition = new Vector3(0, landToPlace.TopPoint, 0);

            Card cardComponent = creatureGO.GetComponent<Card>();
            cardComponent.Data = newCard.Data;
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
