using UnityEngine.Localization.Components;
using UnityEngine;


namespace Tenacity.Dialogs
{
    public class Answer : MonoBehaviour
    {
        [field: SerializeField] public LocalizeStringEvent Text { get; private set; }
    }
}