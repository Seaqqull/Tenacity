using UnityEngine.Localization.Components;
using UnityEngine.UI;
using UnityEngine;
using System;


namespace Tenacity.Dialogs
{
    public class Answer : MonoBehaviour
    {
        [field: SerializeField] public LocalizeStringEvent Text { get; private set; }
        [SerializeField] private Button _button;

        private Action _onAnswerAction;
        
        public event Action AnswerAction
        {
            add { _onAnswerAction += value; }
            remove { _onAnswerAction -= value; }
        }


        private void Awake()
        {
            if (_button != null)
                _button.onClick.AddListener(OnButtonPress);
        }


        private void OnButtonPress()
        {
            _onAnswerAction?.Invoke();
        }
    }
}