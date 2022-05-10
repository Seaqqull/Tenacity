using EngineInput = UnityEngine.Input;
using Tenacity.Battles.Controllers;
using Tenacity.Battles.Data;
using UnityEngine;


namespace Tenacity.Battles.Draggable
{
    public class RayPointerController : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private BattleManager _battle;

        [Header("Ray Line characteristics")] 
        [SerializeField] private LineRenderer _linePrefab;
        [SerializeField] private float _distance = 1000f;        
        [SerializeField] private Vector3 _offset;

        private BattlePlayerController _player;
        private LineRenderer _lineRenderer;
        private Vector3 _targetPos;

        public Vector3 StartPosition { get; set; }


        private void Awake()
        {
            _player = _battle.Player;

            _lineRenderer = Instantiate(_linePrefab, transform);
            _lineRenderer.enabled = false;
        }

        private void Update()
        {
            if ((_battle.CurrentBattleState != BattleState.WaitingForPlayerTurn) || 
                (_player.CurrentPlayerMode == PlayerActionMode.MovingCreature)) return;

            if (_player.CurrentPlayerMode != PlayerActionMode.None)
                DrawPointerLine();
            else if (_lineRenderer.enabled) 
                _lineRenderer.enabled = false;
        }


        private void DrawPointerLine()
        {
            if (!_lineRenderer.enabled)
            {
                Vector3 screenPointPos = StartPosition + _offset;
                _lineRenderer.SetPosition(0, screenPointPos);
                _lineRenderer.positionCount = 1;
                _lineRenderer.enabled = true;
            }
            else
            {
                _targetPos = GetMousePosition().GetValueOrDefault();
                _lineRenderer.positionCount = 2;
                _lineRenderer.SetPosition(1, _targetPos);
                if ((EngineInput.GetMouseButtonDown(0) && !IsHitWithObject(_distance)) || (EngineInput.GetMouseButtonDown(1)))
                {
                    _battle.Player.CurrentPlayerMode = PlayerActionMode.None;
                    _lineRenderer.enabled = false;
                }
            }
        }

        private Vector3? GetMousePosition()
        {
            Vector3 mousePos = EngineInput.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, _distance))
            {
                Debug.Log(hitData.collider.gameObject);
                return hitData.point;
            }
            return Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10.0f));
        }

        private bool IsHitWithObject(float distance)
        {
            Ray ray = Camera.main.ScreenPointToRay(EngineInput.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, distance)) 
                return true;
            return false;
        }
    }
}