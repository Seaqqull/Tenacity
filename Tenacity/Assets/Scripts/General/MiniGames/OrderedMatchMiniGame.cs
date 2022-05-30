using Tenacity.General.MiniGames.Additional;
using System.Collections.Generic;
using Random = System.Random;
using System.Linq;
using UnityEngine;


namespace Tenacity.General.MiniGames
{
    public class OrderedMatchMiniGame : MiniGame
    {
        #region CONSTANTS
        private const float FULL_ROTATION = 360.0f;
        private const float SPHERE_RADIUS = 10;
        #endregion
        
        [SerializeField] private GameObject _toggleParent;
        [SerializeField] private NumericalToggle _toggle;
        [Header("Selection")]
        [SerializeField] [Range(2, 16)] private int _itemsCount;
        [SerializeField] [Range(1, 16)] private int _queueCount = 2;
        [SerializeField] [Range(0.0f, 360.0f)] private float _startAngle; // Beginning of the items distribution
        [SerializeField] [Range(0.0f, 360.0f)] private float _rangeAngle; // Distribution of certain amount of items on the given angle range 
        [SerializeField] [Range(-360.0f, 360.0f)] private float _slotExtensionAngle; // Additional precision angle
        [SerializeField] [Range(0.0f, 360.0f)] private float _slotPrecisionAngle; // Additional precision angle
        [SerializeField] [Range(0.0f, 360.0f)] private float _nonSelectionAngleExtension;
        [Header("Visual")]
        [SerializeField] private float _radiusMax;
        [SerializeField] private float _radiusMin;
        [SerializeField] private AnimationCurve _showAnimation;
        [SerializeField] private AnimationCurve _hideAnimation;
        [Space]
        [Header("Test")]
        [SerializeField] private bool _testDirectionShow;
        [SerializeField] [Range(0.0f, 1.0f)] private float _testProgress;

        private List<NumericalToggle> _queueTogglers = new List<NumericalToggle>();
        private NumericalToggle[] _togglers;
        private Switcher[] _switchers;

        // Selection
        private float _confirmationTimeLeft;
        private int _currentResult;
        private int _targetResult;
        private int _currentQueue;

        // Selection parameters
        private float _halfSlotPrecisionAngle;
        private float _halfNonSelectionAngle;
        private float _halfExtensionAngle;
        private float _halfSlotAngle;
        private float _angleStep;
        

        private void Start()
        {
            if ((_itemsCount % 2) != 0)
                _itemsCount++;
            _targetResult = (_itemsCount / _queueCount) + 1;
            _currentResult = 1;
            _currentQueue = 0;
            
            var togglePositions = GeneratePoints().OrderBy(x => new Random().Next()).ToArray();
            var animationProgress = (_testDirectionShow) ? _showAnimation.Evaluate(_testProgress) :
                _hideAnimation.Evaluate(_testProgress);
            var circleRadius = Mathf.Lerp(_radiusMin, _radiusMax, animationProgress);

            _togglers = new NumericalToggle[_itemsCount];
            for (int i = 0; i < _itemsCount; i++)
            {
                _togglers[i] = Instantiate(_toggle, _toggleParent.transform);
                _togglers[i].Transform.localPosition = Vector3.Scale(togglePositions[i],
                    new Vector3(1.0f, 1.0f) * circleRadius);
                
                _togglers[i].OnClickAction += OnClick;
                _togglers[i].Text = ((i / _queueCount) + 1).ToString();
            }
        }

        private void OnDrawGizmosSelected()
        {
            // -- Slots visualization
            var circlePoints = GeneratePoints();
            var animationProgress = (_testDirectionShow) ? _showAnimation.Evaluate(_testProgress) :
                _hideAnimation.Evaluate(_testProgress);
            var circleRadius = Mathf.Lerp(_radiusMin, _radiusMax, animationProgress);

            foreach (var circlePoint in circlePoints)
            {
                Gizmos.DrawWireSphere(
                    transform.position + Vector3.Scale(circlePoint,
                        new Vector3(1.0f, 1.0f) * circleRadius),
                    SPHERE_RADIUS
                );
            }

            // -- Slot angles visualization
            _angleStep = (_rangeAngle - _nonSelectionAngleExtension) / _itemsCount; // Angle size for each item

            _halfNonSelectionAngle = _nonSelectionAngleExtension * 0.5f;
            _halfSlotPrecisionAngle = _slotPrecisionAngle * 0.5f;
            _halfExtensionAngle = _slotExtensionAngle * 0.5f;
            _halfSlotAngle = _angleStep * 0.5f;

            for (int i = 0; i < _itemsCount; i++)
            {
                var angle = (_startAngle + _halfNonSelectionAngle + ((float)i / (_itemsCount - 1)) * (_rangeAngle - _nonSelectionAngleExtension));

                // Ordinal angles
                var lowerAngle = (angle - _halfSlotAngle - _halfExtensionAngle) % FULL_ROTATION;
                if (lowerAngle < 0.0f) lowerAngle = FULL_ROTATION + lowerAngle;
                var upperAngle = (angle + _halfSlotAngle + _halfExtensionAngle) % FULL_ROTATION;

                // Precision angles
                var lowerPrecisionAngle = (lowerAngle - _halfSlotPrecisionAngle) % FULL_ROTATION;
                if (lowerPrecisionAngle < 0.0f) lowerPrecisionAngle = FULL_ROTATION + lowerPrecisionAngle;
                var upperPrecisionAngle = (upperAngle + _halfSlotPrecisionAngle) % FULL_ROTATION;

                // -- Visualization
                var position = transform.position;
                // Ordinal
                DrawLine(position, lowerAngle, circleRadius, Color.white);
                DrawLine(position, upperAngle, circleRadius, Color.white);
                // Precision
                DrawLine(position, lowerPrecisionAngle, circleRadius, Color.red);
                DrawLine(position, upperPrecisionAngle, circleRadius, Color.red);
            }
        }


        private void ClearQueue()
        {
            foreach (var toggle in _queueTogglers)
                toggle.Switch();
            
            _queueTogglers.Clear();
        }
        
        protected override void Validate()
        {
            if (_currentResult == _targetResult)
                Succeed();
        }
        
        private void OnClick(NumericalToggle toggle)
        {
            if (_currentQueue == 0) // New queue
            {
                if (_currentResult.ToString() == toggle.Text)
                {
                    _currentQueue++;
                    _queueTogglers.Add(toggle);
                }
                else
                {
                    _currentResult = 1;
                    
                    toggle.Switch();
                    ClearQueue();
                }
                return;
            }
            
            // Some item was active previously
            if (_currentResult.ToString() != toggle.Text) // Activated wrong toggle
            {
                _currentResult = 1;
                _currentQueue = 0;
                
                toggle.Switch();
                ClearQueue();
            }
            else // Activated right toggle
            {
                _currentQueue++;
                _queueTogglers.Add(toggle);
                
                if (_currentQueue == _queueCount) // Activated all right items of given queue
                {
                    _currentResult++;
                    _currentQueue = 0;
                }

                Validate();
            }
        }
        
        private IEnumerable<Vector3> GeneratePoints()
        {
            return Enumerable.Range(0, _itemsCount).Select(i =>
                new Vector3(
                    Mathf.Sin(Mathf.Deg2Rad * (_startAngle + ((float)i / (_itemsCount - 1)) * _rangeAngle)),
                    Mathf.Cos(Mathf.Deg2Rad * (_startAngle + ((float)i / (_itemsCount - 1)) * _rangeAngle)),
                    transform.position.z
                )
            );
        }
        
        private void DrawLine(Vector3 from, float directionAngle, float length, Color color)
        {
            var oldColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawLine(
                from,
                from + Vector3.Scale(
                    new Vector3(Mathf.Sin(Mathf.Deg2Rad * directionAngle), Mathf.Cos(Mathf.Deg2Rad * directionAngle)),
                    new Vector3(1.0f, 1.0f) * length)
            );
            Gizmos.color = oldColor;
        }
        
    }
}