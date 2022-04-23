using Tenacity.Battle;
using Tenacity.Draggable;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tenacity.Lands
{
    public class LandDeckItem : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private BattleController _battle;
        [SerializeField] private RayPointerController _rayPointerController;

        private LandDeck _landDeck => GetComponentInParent<LandDeck>();


        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if (!_landDeck.enabled) return;
            if (_battle.CurrentBattleState != BattleController.BattleState.WaitingForPlayerTurn 
                && _battle.Player.CurrentPlayerMode == BattlePlayerController.PlayerActionMode.PlacingLand) 
                return;
            _battle.Player.CurrentlySelectedLand = gameObject.GetComponent<Land>();
            _rayPointerController.StartPosition = transform.position;

        }
    }
}