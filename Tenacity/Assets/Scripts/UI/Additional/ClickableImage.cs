using Tenacity.Utility.Methods;
using UnityEngine.UI;
using UnityEngine;
using System;


namespace Tenacity.UI.Additional
{
    public class ClickableImage : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;

        
        public void AssignAction(Sprite image, Action action)
        {
            _image.sprite = image;
            _image.gameObject.SetActive(image != null);
            
            _button.onClick.SetAction(action);
            _button.interactable = (action != null);
        }
    }
}
