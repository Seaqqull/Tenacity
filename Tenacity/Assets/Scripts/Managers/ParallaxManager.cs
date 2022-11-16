using UnityEngine;


namespace Tenacity.Managers
{
    public class ParallaxManager : MonoBehaviour
    {
        [System.Serializable]
        public struct ParallaxEntity
        {
            public Transform Source;
            public Vector2 MovementScale;

            public Vector3 OriginPosition { get; set; }
        }

        
        [SerializeField] private ParallaxEntity[] _entities;

        private Vector2Int _screenHalfSize;
        private Vector2Int _screenSize;


        private void Awake()
        {
            _screenHalfSize = new Vector2Int(Screen.width / 2, Screen.height / 2);
            _screenSize = new Vector2Int(Screen.width, Screen.height);

            for (int i = 0; i < _entities.Length; i++)
            {
                _entities[i].OriginPosition = _entities[i].Source.localPosition;
            }
        }

        private void Update()
        {
            var relativeMousePosition = GetCentroidMousePosition(InputManager.MousePosition) / _screenHalfSize;

            for (int i = 0; i < _entities.Length; i++)
            {
                var positionShift = relativeMousePosition * _entities[i].MovementScale;
                _entities[i].Source.localPosition =
                    _entities[i].OriginPosition + new Vector3(positionShift.x, positionShift.y, 0.0f);
            }
        }


        private Vector2 GetCentroidMousePosition(Vector2 mousePosition)
        {
            return new Vector2(
                Utility.Methods.FloatHelper.Map(mousePosition.x, 0.0f, _screenSize.x, -_screenHalfSize.x, _screenHalfSize.x), 
                Utility.Methods.FloatHelper.Map(mousePosition.y, 0.0f, _screenSize.y, -_screenHalfSize.y, _screenHalfSize.y)
            );
        }
    }
}
