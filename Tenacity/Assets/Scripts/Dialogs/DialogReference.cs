using UnityEngine;


namespace Tenacity.Dialogs
{
    [CreateAssetMenu(menuName = "Dialogs/Create")]
    public class DialogReference : TextReference
    {
        [field: SerializeField] public Data.DialogOption[] Answers { get; private set; }
    }
}