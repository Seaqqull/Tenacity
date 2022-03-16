using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Tenacity.Cards
{
    public class CardDataDisplay : MonoBehaviour
    {
        private Card card;
        private Dictionary<string, Transform> cardComponents;

        private void Start()
        {
            if (TryGetComponent(out card))
            {
                DisplayCardValues();
            }
        }

        public void DisplayCardValues()
        {
            if (card.Data == null) return;

            cardComponents = transform.GetComponentsInChildren<Transform>().ToDictionary(item => item.name, item => item);
            foreach (var prop in card.Data.CardProperties)
            {
                if (!cardComponents.ContainsKey(prop.Name)) continue;

                var component = cardComponents[prop.Name];
                var value = prop.GetValue(card.Data, null).ToString();
                SetCardValue(component, value);
            }
        }

        public virtual void SetCardValue(Transform component, string value)
        {
            if (component.TryGetComponent(out TextMeshPro cardTextMeshPro))
            {
                cardTextMeshPro.text = value;
            } 
            else if (component.TryGetComponent(out SpriteRenderer cardSprite))
            {
                cardSprite.sprite = Resources.Load<Sprite>($"Sprites/Cards/card_{value}");
            }
        }
    }
}