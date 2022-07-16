using UnityEngine.Events;
using UnityEngine;
using System;


namespace Tenacity.UI.Additional
{
    public class SwitchableButton : MonoBehaviour
    {
        [Serializable] public class BoolEvent : UnityEvent<bool> {}

        
        [SerializeField] protected bool _selected;
        [Header("UI")] 
        [SerializeField] protected GameObject _ordinalBackground;
        [SerializeField] protected GameObject _selectedBackground;
        [Header("Events")] 
        [SerializeField] protected BoolEvent _onSwitch;
        
        public bool Selected
        {
            get => _selected;
        }


        protected void Awake()
        {
            SwitchButtons(_selected);
        }


        protected void SwitchButtons(bool enable)
        {
            _selectedBackground.SetActive(enable);
            _ordinalBackground.SetActive(!enable);
        }

        
        public void SwitchSelection()
        {
            SetSelection(!_selected);
        }

        public virtual void SetSelection(bool selection)
        {
            if (_selected == selection) return;

            _selected = selection;
            SwitchButtons(_selected);
            _onSwitch?.Invoke(_selected);
        }
    }
}
