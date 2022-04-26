using System.Collections;
using Tenacity.Battle;
using UnityEngine;
using EngineInput = UnityEngine.Input;

namespace Tenacity.Draggable
{
    public class RayPointerController : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private BattleManager _battle;

        [Header("Offset Vector values")]
        [SerializeField] private float _offsetX = 0f;
        [SerializeField] private float _offsetY = 7.1f;
        [SerializeField] private float _offsetZ = -5f;

        [Header("Ray Line characteristics")]
        [SerializeField] private float _lineWidth = 0.1f;
        [SerializeField] private float _distance = 1000f;

        private Vector3 _offset;
        private Vector3 _targetPos;
        private LineRenderer _lineRenderer;
        private BattlePlayerController _player;

        public Vector3 StartPosition { get; set; }


        private void Awake()
        {
            _player = _battle.Player;

            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.startWidth = _lineWidth;
            _lineRenderer.endWidth = _lineWidth;
            _lineRenderer.enabled = false;

            _offset = new Vector3(_offsetX, _offsetY, _offsetZ);
        }

        private void Update()
        {
            if (_battle.CurrentBattleState != BattleManager.BattleState.WaitingForPlayerTurn) return;
            if (_player.CurrentPlayerMode == BattlePlayerController.PlayerActionMode.MovingCreature) return;

            if (_player.CurrentPlayerMode != BattlePlayerController.PlayerActionMode.None)
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
                    _battle.Player.CurrentPlayerMode = BattlePlayerController.PlayerActionMode.None;
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
                return hitData.point;
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