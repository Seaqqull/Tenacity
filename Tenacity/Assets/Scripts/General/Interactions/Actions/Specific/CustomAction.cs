using UnityEngine.Events;
using UnityEngine;


namespace Tenacity.General.Interactions.Actions
{
    [CreateAssetMenu(menuName = "Action/Interaction/CustomAction", fileName = "CustomAction", order = 0)]
    public class CustomAction : CompoundInteractionAction
    {
        [SerializeField] private UnityEvent _onClick;


        public void Execute()
        {
            _onClick.Invoke();
        }

        public void Execute(Interaction interaction)
        {
            Execute(interaction, (interaction as GraphicalInteraction).Collision);
        }

        public override void Execute(Interaction interaction, Collider intruder)
        {
            base.Execute(interaction, intruder);
            
            _onClick.Invoke();
        }
    }
}