using Tenacity.Managers;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/BlockMouseClickForPeriod", fileName = "BlockMouseClickForPeriod", order = 0)]
    public class BlockMouseClickForPeriodSO : ActionSO<bool>
    {
        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return true;
        }
        
        
        public void BlockMouse(float period)
        {
            SceneManager.Instance.UnblockMouseWithDelay(period, true);
        }
    }
}