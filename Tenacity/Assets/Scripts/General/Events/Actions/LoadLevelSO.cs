using Tenacity.Properties;
using Tenacity.Managers;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/LoadLevel", fileName = "LoadLevel", order = 0)]
    public class LoadLevelSO : ActionSO<bool>
    {
        [SerializeField] private string _sceneName;
        
        
        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return false;
        }
        
        public void Perform(IntegerVariable levelId)
        {
            SceneManager.Instance.LoadMainGame(levelId.Value, _sceneName);
        }
    }
}