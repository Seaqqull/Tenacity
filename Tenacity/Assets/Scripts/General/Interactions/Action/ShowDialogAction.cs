using Tenacity.Dialogs;
using UnityEngine;


namespace Tenacity.General.Interactions.Action
{
    [CreateAssetMenu(menuName = "Action/Interaction/ShowDialog")]
    public class ShowDialogAction : InteractionAction
    {
        public override void Execute(Interaction interaction, Collider intruder)
        {
            var dialog = interaction.GetComponentInChildren<Dialog>();
            if(dialog != null)
                dialog.ShowDialog();
        }
    }
}