using Tenacity.Battles.Controllers;
using Tenacity.Battles.Draggable;
using Tenacity.Battles.Data;
using UnityEngine.UI;
using UnityEngine;


namespace Tenacity.Battles.Lands
{
    public class LandDeckPlacingController : MonoBehaviour
    {
        [SerializeField] private BattleManager _battle;
        [SerializeField] private RayPointerController _rayPointerController;

        private BattlePlayerController _player;
        private int _availableLansCardsCount;
        private Land _currentlySelectedLand;

        private bool IsRechangable => 
            (_currentlySelectedLand != null && _availableLansCardsCount == BattleConstants.LandConstants.GetLandCellsCount(_currentlySelectedLand.Type));
        public bool IsCurrentlyPlacingLand => _player.CurrentPlayerMode == PlayerActionMode.PlacingLand;
        public Land CurrentlySelectedLand
        {
            get => _currentlySelectedLand;
            set
            {
                if (value != null)
                {
                    _currentlySelectedLand = value;
                    _currentlySelectedLand.GetComponent<Outline>().enabled = true;
                    _availableLansCardsCount = BattleConstants.LandConstants.GetLandCellsCount(value.Type);
                }
                else
                {
                    _currentlySelectedLand.GetComponent<Outline>().enabled = false;
                    _currentlySelectedLand = null;
                    _availableLansCardsCount = 0;
                }
            }
        }

        
        private void Awake()
        {
            _player = _battle.Player;
        }

        private void Update()
        {
            if (CurrentlySelectedLand == null) return;
            if ((CurrentlySelectedLand != null) && !IsRechangable) _player.CurrentPlayerMode = PlayerActionMode.PlacingLand;
            else if (_player.CurrentPlayerMode == PlayerActionMode.None) CurrentlySelectedLand = null;
        }

        
        public void SelectLand(Land land)
        {
            if (_battle.CurrentBattleState != BattleState.WaitingForPlayerTurn) return;
            
            CurrentlySelectedLand = land;
            _player.CurrentPlayerMode = PlayerActionMode.PlacingLand;

            var landRectTransform = land.GetComponent<RectTransform>();
            _rayPointerController.StartPosition = (landRectTransform.TransformPoint(landRectTransform.rect.center));
        }

        public void DecreaseAvailableLandCardsCount()
        {
            _availableLansCardsCount--;
            
            if (_availableLansCardsCount <= 0)
            {
                CurrentlySelectedLand = null;
                _player.CurrentPlayerMode = PlayerActionMode.None;
                this.enabled = false;
            }
        }

    }
}