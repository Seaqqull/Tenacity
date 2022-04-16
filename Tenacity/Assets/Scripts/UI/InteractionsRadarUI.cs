using Tenacity.Utility;
using UnityEngine;


namespace Tenacity.UI
{
    public class InteractionsRadarUI : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private RectTransform _bounds;

        private GameObject[] _marks;
        private Vector3 _boundsSize;
        

        private void Awake()
        {
            _boundsSize = new Vector2(_bounds.rect.x * -0.5f, _bounds.rect.y * -0.5f);
        }


        public void UpdateItems(DetectionTarget[] items)
        {
            if (_marks != null)
                for (int i = 0; i < _marks.Length; i++)
                    Destroy(_marks[i]);
            if (items == null)
                return;
            
            _marks = new GameObject[items.Length];
            for(int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                _marks[i] = Instantiate(_itemPrefab, _bounds, false);
                var scaledPosition = (new Vector3(item.Direction.x, item.Direction.z) )//* item.RelativeDistance
                    .normalized;
                scaledPosition.Scale(_boundsSize);
                _marks[i].transform.localPosition = (scaledPosition * item.RelativeDistance);
            }
        }
    }
}
