using Tenacity.Utility.Interfaces;
using System.Collections;
using Tenacity.Base;
using UnityEngine;
using System;
using TMPro;


namespace Tenacity.General.MiniGames.Additional
{
    public class NumericalToggle : BaseMono, IRunLater
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private bool _textVisible;
        [SerializeField] [Range(0.1f, 1.0f)] private float _showPeriod = 0.1f;
        [Space]
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _enabledState;
        [SerializeField] private GameObject _disabledState;

        private Action<NumericalToggle> _onClick;

        public event Action<NumericalToggle> OnClickAction
        {
            add { _onClick += value; }
            remove { _onClick -= value; }
        }
        public bool TextVisible
        {
            get => _textVisible;
            set 
            {
                _textVisible = value;
                _text.enabled = value;
            }
        }
        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }
        
        
        protected override void Awake()
        {
            base.Awake();
            
            SetState(_enabled);
        }
        
        
        public void Switch()
        {
            SetState(!_enabled);
        }
        
        public void OnClick()
        {
            Switch();
            RunLater(() => _onClick?.Invoke(this), _showPeriod);
        }

        public void SetState(bool enabled)
        {
            TextVisible = enabled;
            _enabled = enabled;
            
            _enabledState.SetActive(enabled);
            _disabledState.SetActive(!enabled);
        }

        
        #region RunLater
        public void RunLater(Action method, float waitSeconds)
        {
            RunLaterValued(method, waitSeconds);
        }

        public Coroutine RunLaterValued(Action method, float waitSeconds)
        {
            if ((waitSeconds < 0) || (method == null))
                return null;

            return StartCoroutine(RunLaterCoroutine(method, waitSeconds));
        }

        public IEnumerator RunLaterCoroutine(Action method, float waitSeconds)
        {
            yield return new WaitForSeconds(waitSeconds);
            method();
        }        
        #endregion
    }
}