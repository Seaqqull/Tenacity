using UnityEngine;


namespace Tenacity.Utility
{
    public class PointFollower : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [Header("Follow parameters")] 
        [SerializeField] [Range(0.0f, 5.0f)] private float _followTime;
        [SerializeField] private Vector3 _offset;

        private Transform _transform;


        private void Awake()
        {
            _transform = transform;
        }
        
        private void LateUpdate()
        {
            _transform.position = Vector3.Lerp(_transform.position, (_followTarget.position + _offset), _followTime);
        }
    }
}