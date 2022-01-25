using UnityEngine;


namespace Tenacity.Dialogs.Data
{
    public enum AnswerAction { Close, NextDialog, Custom }
    public enum AnswerType { Neutral, Good, Bad }

    
    [System.Serializable]
    public class DialogOption
    {
        public TextReference Text;
        [Space] 
        [Header("Parameters")] 
        public AnswerAction Action;
        public AnswerType Type;
        [Range(-100, 100)] public int RelationInfluence;
        [Space]
        public DialogReference NextDialog;
    }
}