using System.Collections.Generic;
using Tenacity.UI.Menus.Views;
using Tenacity.General.Items;
using UnityEngine.UI;
using Tenacity.Cards;
using System.Linq;
using UnityEngine;
using TMPro;


namespace Tenacity.UI.Menus
{
    public class CardView : ItemView
    {
        private Dictionary<string, Transform> _cardComponents;
        private CardSO _card;
        
        public override ItemType ViewType => ItemType.Card;

        
        private void DisplayCardValues()
        {
            if (_card == null) return;

            _cardComponents = Transform.GetComponentsInChildren<Transform>().ToDictionary(item => item.name, item => item);
            foreach (var prop in _card.CardProperties)
            {
                if (!_cardComponents.ContainsKey(prop.Name)) continue;

                var component = _cardComponents[prop.Name];
                var value = prop.GetValue(_card, null).ToString();
                SetCardValue(component, value);
            }
        }

        private void SetCardValue(Transform component, string value)
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
        

        public override void ShowItemData(IItem item)
        {
            _card = item as CardSO;
            DisplayCardValues();
        }

        public override bool IsItemCompatible(IItem item)
        {
            return ((item as CardSO) != null);
        }
    }
}
