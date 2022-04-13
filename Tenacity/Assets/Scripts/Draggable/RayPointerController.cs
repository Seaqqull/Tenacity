using System.Collections;
using Tenacity.Battle;
using UnityEngine;
using EngineInput = UnityEngine.Input;

namespace Tenacity.Draggable
{
    public class RayPointerController : MonoBehaviour
    {
        [SerializeField] private BattleController _battle;
        [SerializeField] private float _lineWidth = 0.1f;
        [SerializeField] private LayerMask _landLayer; //tmp
        [SerializeField] private float _distance; //tmp

        public Vector3 StartPosition { get; set; }

        private Vector3 _targetPos;
        private LineRenderer _lineRenderer;


        private void Awake()
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.startWidth = _lineWidth;
            _lineRenderer.endWidth = _lineWidth;
            _lineRenderer.enabled = false;
        }

        private void Update()
        {
            if (_battle.CurrentBattleState != BattleController.BattleState.WaitingForPlayerTurn) return;

            if (_battle.Player.CurrentPlayerMode != BattlePlayerController.PlayerActionMode.None)
            {
                DrawPointerLine();  
            } 
            else if (_lineRenderer.enabled)
            {
                _lineRenderer.enabled = false;
            }
        }

        private void DrawPointerLine()
        {
            if (!_lineRenderer.enabled)
            {
                _lineRenderer.SetPosition(0, StartPosition);
                _lineRenderer.positionCount = 1;
                _lineRenderer.enabled = true;
            }
            else
            {
                _targetPos = GetMousePosition().GetValueOrDefault();
                _lineRenderer.positionCount = 2;
                _lineRenderer.SetPosition(1, _targetPos);
                if ((EngineInput.GetMouseButtonDown(0)) && (!IsHitWithObject(_landLayer, _distance)))
                {
                    _lineRenderer.enabled = false;
                    _battle.Player.OnClickDisable();
                }
            }
        }

        private Vector3? GetMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(EngineInput.mousePosition);
            var plane = new Plane(_battle.gameObject.transform.position, Vector3.up);
            float rayDist;
            if (plane.Raycast(ray, out rayDist)) return ray.GetPoint(rayDist);
            return null;
        }

        private bool IsHitWithObject(LayerMask layerMask, float distance)
        {
            Ray ray = Camera.main.ScreenPointToRay(EngineInput.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, distance, layerMask);
            return hit.collider != null;
        }
    }
}