using System.Collections.Generic;
using Tenacity.Utility;
using UnityEngine;
using System;


namespace Tenacity.UI
{
    public class InteractionRadarUI : MonoBehaviour
    {
        [Flags]
        private enum Side { None, Left, Right, Top = 4, Bottom = 8 }

        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private RectTransform _bounds;
        [Space] 
        [SerializeField] private Side _boundDirection;
        [SerializeField] [Range(0.0f, 1.0f)]private float _boundsScale = 1.0f;

        private List<GameObject> _marks = new List<GameObject>();
        private Vector3 _boundsSize;
        

        private void Awake()
        {
            var directions = GetDirectionValues(_boundDirection);
            _boundsSize = new Vector2(_bounds.rect.x * _boundsScale * directions.horizontal, _bounds.rect.y * _boundsScale * directions.vertical);
        }


        private (float horizontal, float vertical) GetDirectionValues(Side side)
        {
            return (
                side.HasFlag(Side.Left) ? -1.0f : side.HasFlag(Side.Right) ? 1.0f : 0.0f,
                side.HasFlag(Side.Bottom) ? -1.0f : side.HasFlag(Side.Top) ? 1.0f : 0.0f
            );
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
