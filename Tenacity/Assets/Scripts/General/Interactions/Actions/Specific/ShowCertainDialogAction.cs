using Tenacity.Dialogs;
using UnityEngine;


namespace Tenacity.General.Interactions.Actions
{
    [CreateAssetMenu(menuName = "Action/Interaction/ShowCertainDialog", fileName = "ShowCertainDialog")]
    public class ShowCertainDialogAction : ShowDialogAction
    {
        [SerializeField] private DialogReference _reference;
        
        
        public override void Execute(Interaction interaction, Collider intruder)
        {
            var dialog = interaction.GetComponentInChildren<Dialog>();
            if (dialog == null) return;

            dialog.Reference = _reference;
            dialog.ShowDialog();
        }
    }
}