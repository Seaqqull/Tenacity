using UnityEngine;


namespace Tenacity.General.Interactions.Action
{
    [CreateAssetMenu(menuName = "Action/Interaction/No")]
    public class NoAction : InteractionAction
    {
        public override void Execute(Interaction interaction, Collider intruder) { }
    }
}