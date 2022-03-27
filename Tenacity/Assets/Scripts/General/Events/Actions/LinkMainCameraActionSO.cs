using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/CanvasToCamera", fileName = "CameraToCanvas", order = 0)]
    public class LinkCameraActionSO : ActionSO<bool>
    {
        public override bool Perform()
        {
            return false;
        }
        
        protected override bool PerformAction()
        {
            return false;
        }
        
        protected override bool CheckPossibility()
        {
            return true;
        }

        public void Perform(Canvas inputData)
        {
            inputData.worldCamera = Managers.StorageManager.Instance.Camera;
        }
    }
}