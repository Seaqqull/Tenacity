using Tenacity.Managers;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/RemoveLastDialogFromStack", fileName = "RemoveLastDialogFromStack", order = 0)]
    public class RemoveLastDialogFromStackSO : ActionSO<bool>
    {
        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return false;
        }
        
        public void PerformDialogClosing()
        {
            var activeDialog = SceneManager.Instance.GetLastDialog();
            activeDialog.CloseDialog();
        }
    }
}