using Tenacity.Properties;
using Tenacity.Managers;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/MiniGame", fileName = "MiniGame", order = 0)]
    public class LoadMiniGameSO : ActionSO<bool>
    {
        protected override bool PerformAction()
        {
            return true;
        }

        
        public override bool Perform()
        {
            return true;
        }
        
        public void LoadMiniGame(IntegerVariable miniGameId)
        {
            MiniGameManager.Instance.ShowGame(miniGameId.Value);
        }
    }
}