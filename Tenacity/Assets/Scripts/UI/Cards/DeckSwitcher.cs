using System;
using UnityEngine;
using TMPro;


namespace Tenacity.UI.Cards
{
    public class DeckSwitcher : MonoBehaviour
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private GameObject _enabledState;
        [SerializeField] private GameObject _disabledState;
        [SerializeField] private TMP_Text _text;

        private Action<DeckSwitcher> _onClick;
        
        public event Action<DeckSwitcher> OnClickAction
        {
            add { _onClick += value; }
            remove { _onClick -= value; }
        }
        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }
        

        private void Awake()
        {
            SetState(_enabled);
        }


        public void Switch()
        {
            SetState(!_enabled);
        }


        public void OnClick()
        {
            _onClick?.Invoke(this);
            Switch();
        }

        public void SetState(bool enabled)
        {
            _enabled = enabled;
            
            _enabledState.SetActive(enabled);
            _disabledState.SetActive(!enabled);
        }
    }
}
