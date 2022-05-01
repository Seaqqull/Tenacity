using EngineInput = UnityEngine.Input;
using System.Collections.Generic;
using Tenacity.Battles.Lands;
using Tenacity.Battles.Data;
using Tenacity.Cards;
using UnityEngine;
using System.Linq;


namespace Tenacity.Battles.Controllers
{
    public class CreatureDragging : MonoBehaviour
    {
        [SerializeField] private BattleManager _battle;
        [SerializeField] private float _movingCreatureZPos;
        [SerializeField] private float _detectionDistance;
        
        private Vector3 _selectedCreaturePosition;
        private BattlePlayerController _player;
        private Card _selectedCreature;

        public bool IsCurrentlyMovingCreature =>
            _player.CurrentPlayerMode == PlayerActionMode.MovingCreature;
        public bool IsPlayerTurn => _battle.CurrentBattleState == BattleState.WaitingForPlayerTurn;


        private void Awake()
        {
            _player = _battle.Player;
        }

        private void Update()
        {
            if ((_battle.CurrentBattleState != BattleState.WaitingForPlayerTurn) || 
                (_player.CurrentPlayerMode != PlayerActionMode.MovingCreature) || 
                (_selectedCreature == null)) return;
            
            if (EngineInput.GetMouseButton(0)) MoveCreature();
            if (EngineInput.GetMouseButtonUp(0)) DropCreature();
        }

        
        private void MoveCreature()
        {
            if (_selectedCreature == null) return;

            _selectedCreature.GetComponentInParent<LandCellController>().HighlightNeighbors(_selectedCreature.Data.Land, true);

            var mousePos = EngineInput.mousePosition;
            var creaturePos = new Vector3(mousePos.x, mousePos.y, _movingCreatureZPos);
            _selectedCreature.Transform.position = Camera.main.ScreenToWorldPoint(creaturePos);
        }

        private void DropCreature()
        {
            _selectedCreature.GetComponentInParent<LandCellController>().HighlightNeighbors(_selectedCreature.Data.Land, false);

            var detectedObjects = GetDetectedObjectsHitWithRaycast(_detectionDistance);
            if (detectedObjects.Count == 0) GetBackSelectedCreature();

            Land detectedLand = detectedObjects
                .Select(go => go.GetComponent<Land>()).FirstOrDefault(el => el != null);

            if ((detectedLand == null) || 
                (!_selectedCreature.GetComponentInParent<Land>().NeighborListContains(detectedLand))) 
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

        private void DeselectCreature()
        {
            _selectedCreature = null;
            _player.CurrentPlayerMode = PlayerActionMode.None;
        }
        
        private void PlaceCreature(Land land)
        {
            _selectedCreature.Transform.SetParent(land.transform);
            _selectedCreature.Transform.localPosition = new Vector3(0, land.TopPoint, 0);
            _selectedCreature.enabled = false;
            DeselectCreature();
        }
        
        private void GetBackSelectedCreature()
        {
            _selectedCreature.Transform.position = _selectedCreaturePosition;
            DeselectCreature();
        }
        
        private void DropCreatureOnEnemy(Card enemyCard)
        {
            var targetCreature = enemyCard.gameObject.GetComponentInChildren<Card>();
            if (!_player.PlayerCards.Contains(targetCreature))
                 _selectedCreature.gameObject.GetComponent<CreatureController>()?.Attack(targetCreature);
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
            _selectedCreaturePosition = card.Transform.position;
            _player.CurrentPlayerMode = PlayerActionMode.MovingCreature;
        }
    }
}