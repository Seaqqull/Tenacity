using System.Collections;
using UnityEngine.Events;
using Tenacity.Utility;
using UnityEngine;


namespace Tenacity.General.Interactions
{
    public class InteractionRadar : Base.BaseMono
    {
        [System.Serializable]
        public class HitInfo : DetectionTarget
        {
        }
        
        [System.Serializable] public class DetectionUnityEvent : UnityEvent<DetectionTarget[]> { }

        [SerializeField] [Range(0.015f, 1.0f)] private float _searchPeriod;
        [SerializeField] [Range(0.0f, short.MaxValue)] private float _range;
        [SerializeField] private DetectionUnityEvent _onTragetUpdate;
        [SerializeField] private LayerMask _target;
        
        private Collider[] _interactableItems = new Collider[10]; 
        private Coroutine _searchItemsRoutine;
        
        [field: SerializeField] public HitInfo[] HitInfos { get; private set; }


        private void Start()
        {
            _searchItemsRoutine = StartCoroutine(SearchForItemsCoroutine());
        }

        private void OnDestroy()
        {
            StopCoroutine(_searchItemsRoutine);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _range);
        }


        private IEnumerator SearchForItemsCoroutine()
        {
            var waitTime = new WaitForSeconds(_searchPeriod);

            while (true)
            {
                yield return waitTime;

                var itemsFound = Physics.OverlapSphereNonAlloc(Transform.position, _range, _interactableItems, _target);
                if (itemsFound == 0)
                {
                    _onTragetUpdate.Invoke(null);
                    HitInfos = null;
                    continue;
                }
                
                HitInfos = new HitInfo[itemsFound];
                for (int i = 0; i < itemsFound; i++)
                {
                    var item = _interactableItems[i].GetComponent<Interaction>();
                    if (item == null)
                        continue;

                    var itemDirection = (Transform.position - item.Position);
                    HitInfos[i] = new HitInfo()
                    {
                        Direction = (itemDirection).normalized, 
                        RelativeDistance = Mathf.Clamp((itemDirection.magnitude / _range), 0.0f, 1.0f)
                    };
                }
                
                _onTragetUpdate.Invoke(HitInfos);
            }
        }
    }
}
