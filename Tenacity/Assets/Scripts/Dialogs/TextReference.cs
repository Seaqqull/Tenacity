using UnityEngine.Localization;
using UnityEngine;


namespace Tenacity.Dialogs
{
    [CreateAssetMenu(menuName = "Dialogs/Text")]
    public class TextReference : ScriptableObject
    {
        [field: SerializeField] public LocalizedString StringReference { get; private set; }
    }
}