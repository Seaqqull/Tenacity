using System.Collections.Generic;
using System.Linq;
using Tenacity.Battle;
using Tenacity.Lands;
using UnityEngine;
using EngineInput = UnityEngine.Input;

namespace Tenacity.Cards
{
    public class CreatureDragging : MonoBehaviour
    {
        [SerializeField] private BattleManager _battle;
        [SerializeField] private float _movingCreatureZPos;
        [SerializeField] private float _detectionDistance;


        private Card _selectedCreature;
        private BattlePlayerController _player;
        private Vector3 _selectedCreaturePosition;


        public bool IsCurrentlyMovingCreature =>
            _player.CurrentPlayerMode == BattlePlayerController.PlayerActionMode.MovingCreature;
        public bool IsPlayerTurn => _battle.CurrentBattleState == BattleManager.BattleState.WaitingForPlayerTurn;


        private void Awake()
        {
            _player = _battle.Player;
        }

        private void Update()
        {
            if (_battle.CurrentBattleState != BattleManager.BattleState.WaitingForPlayerTurn) return;
            if (_player.CurrentPlayerMode != BattlePlayerController.PlayerActionMode.MovingCreature) return;
            if (_selectedCreature == null) return;
            if (EngineInput.GetMouseButton(0)) MoveCreature();
            if (EngineInput.GetMouseButtonUp(0)) DropCreature();
        }

        private void MoveCreature()
        {
            if (_selectedCreature == null) return;

            _selectedCreature.GetComponentInParent<LandCellController>().HighlightNeighbors(_selectedCreature.Data.Land, true);

            var mousePos = EngineInput.mousePosition;
            var creaturePos = new Vector3(mousePos.x, mousePos.y, _movingCreatureZPos);
            _selectedCreature.transform.position = Camera.main.ScreenToWorldPoint(creaturePos);
        }

        private void DropCreature()
        {
            _selectedCreature.GetComponentInParent<LandCellController>().HighlightNeighbors(_selectedCreature.Data.Land, false);

            List<GameObject> detectedObjects = GetDetectedObjectsHitWithRaycast(_detectionDistance);
            if (detectedObjects.Count == 0) GetBackSelectedCreature();

            Land detectedLand = detectedObjects.Select(go => go.GetComponent<Land>()).Where(el => el != null).FirstOrDefault();

            if ( (detectedLand == null) || (!_selectedCreature.GetComponentInParent<Land>().NeighborListContains(detectedLand))) 
            {
                GetBackSelectedCreature();
                return;
            }
            if (detectedLand.IsAvailableForCards && detectedLand.Type.HasFlag(_selectedCreature.Data.Land))
            {
                PlaceCreature(detectedLand);
                return;
            }
            if (detectedLand.GetComponentInChildren<Card>())
                DropCreatureOnEnemy(detectedLand.GetComponentInChildren<Card>());

            GetBackSelectedCreature();
        }

        private void DropCreatureOnEnemy(Card enemyCard)
        {
            var targetCreature = enemyCard.gameObject.GetComponentInChildren<Card>();
            if (!_player.PlayerCards.Contains(targetCreature))
                 _selectedCreature.gameObject.GetComponent<CreatureController>()?.Attack(targetCreature);
        }
        private void DeselectCreature()
        {
            _selectedCreature = null;
            _player.CurrentPlayerMode = BattlePlayerController.PlayerActionMode.None;
        }
        private void PlaceCreature(Land land)
        {
            _selectedCreature.transform.SetParent(land.transform);
            _selectedCreature.transform.localPosition = new Vector3(0, land.TopPoint, 0);
            _selectedCreature.enabled = false;
            DeselectCreature();
        }
        private void GetBackSelectedCreature()
        {
            _selectedCreature.transform.position = _selectedCreaturePosition;
            DeselectCreature();
        }

        private List<GameObject> GetDetectedObjectsHitWithRaycast(float distance)
        {
            Ray ray = Camera.main.ScreenPointToRay(EngineInput.mousePosition);
            RaycastHit[] hitObjects = Physics.RaycastAll(ray.origin, ray.direction, distance);
            if (hitObjects?.Length == 0) return null;

            return hitObjects.Select(go => go.collider?.gameObject).ToList();
        }


        public void SelectCreature(Card card)
        {
            if (!_player.PlayerCards.Contains(card)) return;

            _selectedCreature = card;
            _selectedCreaturePosition = card.transform.position;
            _player.CurrentPlayerMode = BattlePlayerController.PlayerActionMode.MovingCreature;
        }
    }
}