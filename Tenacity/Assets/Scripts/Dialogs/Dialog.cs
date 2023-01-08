using UnityEngine.Localization.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization;
using Tenacity.Dialogs.Data;
using UnityEngine.Events;
using UnityEngine;


namespace Tenacity.Dialogs
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _title;
        [SerializeField] private LocalizeStringEvent _text;
        [SerializeField] private DialogReference _dialog;
        [SerializeField] private GameObject _actionObject;
        [Header("Answer")]
        [SerializeField] private Answer _answerPrefab;
        [SerializeField] private GameObject _answersParent;
        [SerializeField] private AnswerResponse[] _reponses;
        [Header("Events")] 
        [SerializeField] private UnityEvent _onInitialize;
        [SerializeField] private UnityEvent _onShow;
        [SerializeField] private UnityEvent _onHide;

        private List<Answer> _availableAnswers =
            new ();
        private bool _initialized;
        private bool _showed;

        public DialogReference Reference
        {
            get => _dialog;
            set => _dialog = value;
        }


        private void OnDisable()
        {
            if (_showed)
                CloseDialog();
        }

        private void ClearDialog()
        {
            _text.StringReference = new LocalizedString();
            
            // Clear available answers
            for (int i = (_availableAnswers.Count - 1); i >= 0; i--)
            {
                Destroy(_availableAnswers[i].gameObject);
                _availableAnswers.RemoveAt(i);
            }
        }

        private void UpdateDialogData()
        {
            ClearDialog();
            
            
            // Update dialog text
            _title.StringReference = _dialog.TitleReference;
            _text.StringReference = _dialog.StringReference;
            
            // Generate answers
            int answerIndex = 0;
            foreach (var answer in _dialog.Answers)
            {
                int currentAnswerIndex = answerIndex++;
                var newAnswer = Instantiate<Answer>(_answerPrefab, _answersParent.transform);
                newAnswer.Text.StringReference = answer.Text.StringReference;

                if (answer.Action == AnswerAction.NextDialog)
                    newAnswer.AnswerAction += () => { NextDialog(currentAnswerIndex); };
                else if (answer.Action == AnswerAction.Close)
                    newAnswer.AnswerAction += CloseDialog;
                else if (answer.Action == AnswerAction.Custom)
                {
                    var response = _reponses.SingleOrDefault((response) => response.AnswerId.Value == answer.SpecialId.Value);
                    newAnswer.AnswerAction += (response == null) ? CloseDialog : response.Action.Invoke;
                }

                _availableAnswers.Add(newAnswer);
            }
        }
        
        private void NextDialog(int answerIndex)
        {
            if (_dialog.Answers[answerIndex].NextDialog == null)
                Debug.Log($"{Utility.Constants.Debug.DIALOG_SYSTEM}: There is no next dialog");
            else
                _dialog = _dialog.Answers[answerIndex].NextDialog;
            
            UpdateDialogData();
        }

        
        public void ShowDialog() // Here we can send dialog storage
        {
            if (!_initialized)
            {
                _initialized = true;
                _onInitialize?.Invoke();
            }

            
            _showed = true;
            
            UpdateDialogData();
            _actionObject.SetActive(true);
            _onShow?.Invoke();
        }

        public void CloseDialog()
        {
            _showed = false;
            
            _actionObject.SetActive(false);
            _onHide?.Invoke();
        }
    }
}