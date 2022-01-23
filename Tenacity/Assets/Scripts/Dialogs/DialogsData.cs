using UnityEngine;


namespace Tenacity.Dialogs.Data
{
    [System.Serializable]
    public class DialogOption
    {
        public TextReference Text;
        [Range(-100, 100)] public int RelationInfluence;
        [Space]
        public DialogReference NextDialog;
    }
}