using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/NewLevel", fileName = "NewLevel", order = 0)]
    public class NewLevelActionSO : ActionSO<bool>
    {
        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return true;
        }
        
        public void PerformStorageUpdate()
        {
            Managers.StorageManager.Instance.UpdateGameObjects();
        }
    }
}