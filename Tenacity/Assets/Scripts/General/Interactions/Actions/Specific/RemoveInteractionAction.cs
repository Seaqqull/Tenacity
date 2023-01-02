using UnityEngine;


namespace Tenacity.General.Interactions.Actions
{
    [CreateAssetMenu(fileName = "RemoveInteraction", menuName = "Action/Interaction/RemoveInteraction", order = 0)]
    public class RemoveInteractionAction : InteractionAction
    {
        public override void Execute(Interaction interaction, Collider intruder)
        {
            Destroy(interaction);
        }
    }
}