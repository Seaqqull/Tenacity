using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            if (card == null ||  card.Data == null) return;

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
            else if (component.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI textField))
            {
                textField.text = value;
            }
            else if (component.TryGetComponent<Image>(out Image image))
            {
                image.sprite = Resources.Load<Sprite>($"Sprites/Cards/card_{value}");
            }
        }

        public void UpdateLife()
        {
            if (card == null) TryGetComponent(out card);
            cardComponents = transform.GetComponentsInChildren<Transform>().ToDictionary(item => item.name, item => item);
            SetCardValue(cardComponents[nameof(Card.Data.Life)], card.CurrentLife.ToString());
        }
    }
}