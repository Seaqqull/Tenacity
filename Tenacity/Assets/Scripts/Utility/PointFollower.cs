using UnityEngine;


namespace Tenacity.Utility
{
    public class PointFollower : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [Header("Follow parameters")] 
        [SerializeField] [Range(0.0f, 5.0f)] private float _followTime;
        [SerializeField] private Vector3 _offset;

        [Header("Follow axis")] 
        [SerializeField] private bool _x;
        [SerializeField] private bool _y;
        [SerializeField] private bool _z;

        private Transform _transform;
        private Vector3 _alignVector;


        private void Awake()
        {
            _transform = transform;
            _alignVector = new Vector3(
                (_x) ? 1.0f : 0.0f,
                (_y) ? 1.0f : 0.0f,
                (_z) ? 1.0f : 0.0f
            );
        }
        
        private void LateUpdate()
        {
            _transform.position = Vector3.Lerp(
                _transform.position, 
                _transform.position + Vector3.Scale(_alignVector, (_offset + (_followTarget.position - _transform.position))), 
                _followTime);
        }
    }
}