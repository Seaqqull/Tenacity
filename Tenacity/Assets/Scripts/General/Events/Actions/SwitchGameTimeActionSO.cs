using Tenacity.Managers;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/SwitchGameTimeClick", fileName = "SwitchGameTimeClick", order = 0)]
    public class SwitchGameTimeActionSO : ActionSO<bool>
    {
        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return true;
        }

        public void SwitchGameTime(bool timePaused)
        {
            if(timePaused)
                EnvironmentManager.Instance.PauseGameTime();
            else
                EnvironmentManager.Instance.ResumeGameTime();
        }
    }
}