using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tenacity.Cards
{
    public class CardPackManager : MonoBehaviour
    {
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform[] cardPositions;
        [SerializeField] private List<CardData> cardPack = new List<CardData>();

        private bool[] availableCardPositions;
        private IEnumerator cardDrawingCoroutine;

        private readonly float _coroutineTime = 0.5f;

        private void Start()
        {
            availableCardPositions = Enumerable.Range(0, cardPositions.Length).Select(x => true).ToArray();

            cardDrawingCoroutine = DrawCardPack(_coroutineTime);
            StartCoroutine(cardDrawingCoroutine);
        }

        private IEnumerator DrawCardPack(float waitTime)
        {
            while (cardPack.Count > 0)
            {
                PlaceCardOnAvailableSlot();
                yield return new WaitForSeconds(waitTime);
            }
        }

        private void PlaceCardOnAvailableSlot()
        {
            if (cardPack.Count <= 0) return;

            for (int i = 0; i < availableCardPositions.Length; i++)
            {
                if (availableCardPositions[i])
                {
                    CreateCardOnDeckFromPackItems(i);
                    return;
                }
            }
        }
        private void CreateCardOnDeckFromPackItems(int slotId)
        {
            Card card = Instantiate(cardPrefab, cardPositions[slotId]);
            var cardData = cardPack[Random.Range(0, cardPack.Count)];

            card.Data = cardData;
            card.transform.parent = cardPositions[slotId];
            card.gameObject.SetActive(true);

            cardPack.Remove(cardData);
            availableCardPositions[slotId] = false;
        }
    }
}