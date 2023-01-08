using UnityEngine;


namespace Tenacity.General.Interactions.Actions.Chest
{
    [CreateAssetMenu(fileName = "HideInteraction", menuName = "Action/Interaction/HideInteraction", order = 0)]
    public class HideInteractionAction : RemoveInteractionAction
    {
        public override void Execute(Interaction interaction, Collider intruder)// TODO: Rebuild for later invocations
        {
            var collider = interaction.GetComponentInChildren<Collider>();
            if (collider != null)
                collider.enabled = false;
            
            
            base.Execute(interaction, intruder);
        }
    }
}