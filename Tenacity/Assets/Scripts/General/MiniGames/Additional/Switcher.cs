using UnityEngine;
using System;


namespace Tenacity.General.MiniGames.Additional
{
    
    public class Switcher : MonoBehaviour
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private GameObject _enabledState;
        [SerializeField] private GameObject _disabledState;

        private Func<bool> _switchAllowed;
        private Action<bool> _onSwitch;
        
        public event Func<bool> SwitchAllowed
        {
            add { _switchAllowed += value; }
            remove { _switchAllowed -= value; }
        }
        public event Action<bool> OnSwitch
        {
            add { _onSwitch += value; }
            remove { _onSwitch -= value; }
        }

            
        private void Awake()
        {
            SetState(_enabled);
        }

        private void OnDestroy()
        {
            _onSwitch = null;
        }


        public void Switch()
        {
            SetState(!_enabled);
        }

        public void SetState(bool enabled)
        {
            if (!_switchAllowed?.Invoke() ?? false)
                return;
            
            _enabled = enabled;
            
            _enabledState.SetActive(enabled);
            _disabledState.SetActive(!enabled);
            
            _onSwitch?.Invoke(_enabled);
        }
    }
}