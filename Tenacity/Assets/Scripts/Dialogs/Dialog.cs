using UnityEngine.Localization.Components;
using System.Collections.Generic;
using UnityEngine;


namespace Tenacity.Dialogs
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _text;
        [SerializeField] private DialogReference _dialog;
        [Header("Answer")]
        [SerializeField] private Answer _answerPrefab;
        [SerializeField] private GameObject _answersParent;

        private List<Answer> _availableAnswers =
            new List<Answer>();


        private void Start()
        {
            UpdateDialogData();
        }


        private void UpdateDialogData()
        {
            // Clear available answers
            for (int i = (_availableAnswers.Count - 1); i >= 0; i--)
            {
                Destroy(_availableAnswers[i].gameObject);
                _availableAnswers.RemoveAt(i);
            }

            // Generate answers
            foreach (var answer in _dialog.Answers)
            {
                var newAnswer = Instantiate<Answer>(_answerPrefab, _answersParent.transform);
                newAnswer.Text.StringReference = answer.Text.StringReference;
            }
            
            // Update dialog text
            _text.StringReference = _dialog.StringReference;
        }

    }
}