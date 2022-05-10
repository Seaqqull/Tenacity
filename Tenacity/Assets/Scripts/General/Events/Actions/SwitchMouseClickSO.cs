using Tenacity.Managers;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/SwitchMouseClick", fileName = "SwitchMouseClick", order = 0)]
    public class SwitchMouseClickSO : ActionSO<bool>
    {
        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return true;
        }

        public void BlockMouse(bool mouseBlocked)
        {
            if(mouseBlocked)
                SceneManager.Instance.MouseClickBlocked = true;
            else
                SceneManager.Instance.UnblockMouseWithDelay();
            SceneManager.Instance.MouseHoverVisible = !mouseBlocked;
        }
    }
}