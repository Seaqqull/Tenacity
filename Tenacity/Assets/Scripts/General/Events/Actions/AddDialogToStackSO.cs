using Tenacity.Dialogs;
using Tenacity.Managers;
using UnityEngine;

namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/AddDialogToStack", fileName = "AddDialogToStack", order = 0)]
    public class AddDialogToStackSO : ActionSO<bool>
    {
        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return false;
        }
        
        public void Perform(Dialog dialog)
        {
            SceneManager.Instance.AttachDialog(dialog);
        }
    }
}