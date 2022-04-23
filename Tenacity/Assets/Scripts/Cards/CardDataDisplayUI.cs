using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tenacity.Cards
{
    public class CardDataDisplayUI : CardDataDisplay
    {

        public override void SetCardValue(Transform component, string value)
        {
            if (component.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI textField))
            {
                textField.text = value;
            }
            else if (component.TryGetComponent<Image>(out Image image))
            {
                image.sprite = Resources.Load<Sprite>($"Sprites/Cards/card_{value}");
            }
        }
    }
}
