using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tenacity.Cards
{
    public class BattleCardController : MonoBehaviour
    {
        public static void CreateCardCreatureOnBoard(Card newCard)
        {
            GameObject loadedGO = LoadFromDatabase(newCard);
            if (loadedGO == null) return;

            var cardCreatureGO = Instantiate(loadedGO) as GameObject;
            cardCreatureGO.transform.SetParent(newCard.transform.parent);
            cardCreatureGO.transform.localPosition = newCard.transform.localPosition;

            Card cardComponent = cardCreatureGO.GetComponent<Card>();
            cardComponent.Data = newCard.Data;
            cardComponent.IsDraggable = newCard.IsDraggable;
            cardComponent.State = CardState.OnBoard;
            Destroy(newCard.gameObject);
        }

        private static GameObject LoadFromDatabase(Card newCard)
        {
            return AssetDatabase.LoadAssetAtPath(
                $"Assets/StaticAssets/Prefabs/Creatures/{newCard.Data.Type}_{newCard.Data.CardId}.prefab", typeof(GameObject)
                ) as GameObject;
        }
    }
}
