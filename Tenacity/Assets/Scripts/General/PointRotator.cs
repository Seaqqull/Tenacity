using System.Collections;
using UnityEngine;


namespace Tenacity.General
{
    public class PointRotator : MonoBehaviour
    {
        #region Constants
        private float ALIGN_PERIOD = 0.5f;
        #endregion
        
        [Header("Follow parameters")] 
        [SerializeField] [Range(0.0f, 5.0f)] private float _followTime;

        private Transform _followTarget;
        private Transform _transform;
        private Vector3 _alignVector;


        private void Awake()
        {
            _followTarget = Camera.main.transform;
            _transform = transform;

            StartCoroutine(nameof(AlignRoutine));
        }

        private void OnDestroy()
        {
            StopCoroutine(nameof(AlignRoutine));
        }


        private void UpdateTarget()
        {
            if (_followTarget == null)
                _followTarget = Camera.main.transform;
        }


        private IEnumerator AlignRoutine()
        {
            var waitTime = new WaitForSeconds(ALIGN_PERIOD);
            while(true)
            {
                UpdateTarget();
                if (_followTarget == null) continue;
                
                var currentPosition = _transform.position;
                var viewDirection = (currentPosition - _followTarget.position);
                
                if (viewDirection != Vector3.zero)
                {
                    _transform.LookAt(currentPosition +
                                      Vector3.Lerp(Vector3.zero, viewDirection, _followTime));
                }
                
                yield return waitTime;
            }
        }
    }
}