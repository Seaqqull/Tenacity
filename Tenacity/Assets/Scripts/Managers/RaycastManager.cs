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
            public LayerMask TargetLayer;
            public LayerMask ObstacleLayer;
        }

        [SerializeField] private RaycastInfo _interaction;
        [SerializeField] private RaycastInfo _movement;
        [SerializeField] private RaycastInfo _ground;


        private bool CheckForObstacle(GameObject selectedObject, LayerMask obstacleMask)
        {
            return (((1 << selectedObject.layer) & obstacleMask) != 0);
        }

        private (bool HitSomePosition, MouseHitInfo HitData) GetHitInfo(Ray ray, RaycastInfo rayProperty)
        {
            // Whether ray didn't targeted layer
            if(!Physics.Raycast(ray, out var hit, rayProperty.Distance, rayProperty.TargetLayer | rayProperty.ObstacleLayer) ||
               CheckForObstacle(hit.collider.gameObject, rayProperty.ObstacleLayer))
                return (false, null);
            return (true, new MouseHitInfo() { Position = hit.point, Normal = hit.normal, Object = hit.collider.gameObject});
        }

        private (bool HitSomePosition, MouseHitInfo HitData) GetHitInfoFromCamera(RaycastInfo rayProperty)
        {
            if (Managers.StorageManager.Instance.MainCamera == null)
                return (false, null);
            
            var ray = Managers.StorageManager.Instance.MainCamera.ScreenPointToRay(InputManager.MousePosition);
            return GetHitInfo(ray, rayProperty);
        }
        
        private (bool HitSomePosition, MouseHitInfo HitData) GetHitInfoFromPoint(Vector3 position, RaycastInfo rayProperty)
        {
            if (Managers.StorageManager.Instance.MainCamera == null)
                return (false, null);
            
            var ray = new Ray(
                Managers.StorageManager.Instance.MainCamera.transform.position,
                (position - Managers.StorageManager.Instance.MainCamera.transform.position)
            );
            return GetHitInfo(ray, rayProperty);
        }

        
        public (bool HitSomePosition, MouseHitInfo HitData) GetGroundPoint()
        {
            return GetHitInfoFromCamera(_ground);
        }
        
        public (bool HitSomePosition, MouseHitInfo HitData) GetGroundPoint(Vector3 position)
        {
            return GetHitInfoFromPoint(position, _ground);
        }
        
        public (bool HitSomePosition, MouseHitInfo HitData) GetInteractionPoint()
        {
            return GetHitInfoFromCamera(_interaction);
        }
        
        public (bool HitSomePosition, MouseHitInfo HitData) GetInteractionPoint(Vector3 position)
        {
            return GetHitInfoFromPoint(position, _interaction);
        }

        public (bool HitSomePosition, MouseHitInfo HitData) GetMovementPoint()
        {
            return GetHitInfoFromCamera(_movement);
        }
        
        public (bool HitSomePosition, MouseHitInfo HitData) GetMovementPoint(Vector3 position)
        {
            return GetHitInfoFromPoint(position, _movement);
        }
    }
}
