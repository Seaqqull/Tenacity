using UnityEngine.Localization.Components;
using System.Collections.Generic;
using UnityEngine.Localization;
using Tenacity.Dialogs.Data;
using UnityEngine;


namespace Tenacity.Dialogs
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _text;
        [SerializeField] private DialogReference _dialog;
        [SerializeField] private GameObject _actionObject;
        [Header("Answer")]
        [SerializeField] private Answer _answerPrefab;
        [SerializeField] private GameObject _answersParent;

        private List<Answer> _availableAnswers =
            new List<Answer>();
        private bool _initialized;
        

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

        
        public void ShowDialog()
        {
            UpdateDialogData();
        }

        public void CloseDialog()
        {
            _actionObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}