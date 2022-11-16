using UnityEngine.Events;
using UnityEngine;


namespace Tenacity.General.Interactions.Actions
{
    [CreateAssetMenu(menuName = "Action/Interaction/CustomAction", fileName = "CustomAction", order = 0)]
    public class CustomAction : InteractionAction
    {
        [SerializeField] private UnityEvent _onClick;
        
        
        public override void Execute(Interaction interaction, Collider intruder)
        {
            _onClick.Invoke();
        }
    }
}