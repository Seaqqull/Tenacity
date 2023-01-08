using UnityEngine;


namespace Tenacity.General.Interactions.Actions
{
    [CreateAssetMenu(menuName = "Action/Interaction/CompoundAction", fileName = "CompoundAction", order = 0)]
    public class CompoundInteractionAction : InteractionAction
    {
        [SerializeField] private InteractionAction[] _actions;
        
        
        public override void Execute(Interaction interaction, Collider intruder)
        {
            foreach (var action in _actions)
            {
                action.Execute(interaction, intruder);
            }
        }
    }
}