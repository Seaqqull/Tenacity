using Tenacity.Properties;
using UnityEngine;
using UnityEngine.Events;


namespace Tenacity.Dialogs.Data
{
    public enum AnswerAction { Close, NextDialog, Custom }
    public enum AnswerType { Neutral, Good, Bad }

    
    [System.Serializable]
    public class DialogOption
    {
        public TextReference Text;
        [Space] [Header("Parameters")] 
        public IntegerReference SpecialId;
        public AnswerAction Action;
        public AnswerType Type;
        [Range(-100, 100)] public int RelationInfluence;
        [Space]
        public DialogReference NextDialog;
    }

    [System.Serializable]
    public class AnswerResponse
    {
        public IntegerReference AnswerId;
        public UnityEvent Action;
    }
}