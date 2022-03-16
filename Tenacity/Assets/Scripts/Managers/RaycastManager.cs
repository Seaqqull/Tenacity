using Tenacity.Utility.Data;
using UnityEngine;


namespace Tenacity.Managers
{
    public class RaycastManager : Base.SingleBehaviour<RaycastManager>
    {
        [System.Serializable]
        private class RaycastInfo
        {
            public float Distance;
            public LayerMask Layer;
        }

        [Header("Movement")] 
        [SerializeField] private RaycastInfo _ground;
        
        
        public (bool HitSomePosition, MouseHitInfo HitData) GetMovementPoint()
        {
            if (Managers.StorageManager.Instance.Camera == null)
                return (false, null);
            
            var ray = Managers.StorageManager.Instance.Camera.ScreenPointToRay(InputManager.MousePosition);
            // Whether ray didn't hit anything
            if(!Physics.Raycast(ray, out var hit, _ground.Distance, _ground.Layer))
                return (false, null);
            return (true, new MouseHitInfo() { Position = hit.point, Normal = hit.normal});
        }
        
        public (bool HitSomePosition, MouseHitInfo HitData) GetMovementPoint(Vector3 position)
        {
            if (Managers.StorageManager.Instance.Camera == null)
                return (false, null);
            
            var ray = new Ray(
                Managers.StorageManager.Instance.Camera.transform.position,
                (position - Managers.StorageManager.Instance.Camera.transform.position)
            );
            // Whether ray didn't hit anything
            if(!Physics.Raycast(ray, out var hit, _ground.Distance, _ground.Layer))
                return (false, null);
            return (true, new MouseHitInfo() { Position = hit.point, Normal = hit.normal});
        }
    }
}
