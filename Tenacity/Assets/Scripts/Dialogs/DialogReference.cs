using UnityEngine.Localization;
using UnityEngine;


namespace Tenacity.Dialogs
{
    [CreateAssetMenu(menuName = "Dialogs/Create")]
    public class DialogReference : TextReference
    {
        [field: SerializeField] public LocalizedString TitleReference { get; private set; }
        
        [field: SerializeField] public Data.DialogOption[] Answers { get; private set; }
    }
}