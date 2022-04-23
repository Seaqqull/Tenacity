using System.Collections.Generic;
using Tenacity.Utility;
using UnityEngine;


namespace Tenacity.UI
{
    public class InteractionRadarUI : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private RectTransform _bounds;
        [Space] 
        [SerializeField] [Range(0.0f, 1.0f)]private float _boundsScale = 1.0f; 

        private List<GameObject> _marks = new List<GameObject>();
        private Vector3 _boundsSize;
        

        private void Awake()
        {
            _boundsSize = new Vector2(_bounds.rect.x * -_boundsScale, _bounds.rect.y * -_boundsScale);
        }


        public void UpdateItems(DetectionTarget[] items)
        {
            // Remove redundant
            for (int i = ((items == null) ? (_marks.Count - 1) : (_marks.Count - items.Length)); i >= 0; i--)
            {
                Destroy(_marks[i]);
                _marks.RemoveAt(i);
            }
            if (items == null)
                return;
            // Add requested
            for (int i = (items.Length - _marks.Count); i > 0; i--)
                _marks.Add(Instantiate(_itemPrefab, _bounds, false));

            for(int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                var scaledPosition = (new Vector3(item.Direction.x, item.Direction.z))//* item.RelativeDistance
                    .normalized;
                scaledPosition.Scale(_boundsSize);
                _marks[i].transform.localPosition = (scaledPosition * item.RelativeDistance);
            }
        }
    }
}
