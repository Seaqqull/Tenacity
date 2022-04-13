using System.Collections;
using System.Collections.Generic;
using Tenacity.Battle;
using UnityEngine;

namespace Tenacity.Lands
{
    public class LandController : MonoBehaviour
    {
        private Land _land;
        private BattleController _battle;

        private void Start()
        {
            _land = GetComponent<Land>();
            _battle = transform.parent?.GetComponent<BattleController>();
        }

        public void HighlightNeighbors(LandType selectedType, bool highlighted)
        {
            if (!enabled) return;
            
            _land.OutlineLand(highlighted);
            foreach (Land neighbor in _land.NeighborLands)
            {
                if (neighbor.Type.HasFlag(selectedType) && neighbor.IsAvailableForCards)
                    neighbor.OutlineLand(highlighted);
            }
        }

        public void OnMouseDown()
        {
            if (_battle.CurrentBattleState == BattleController.BattleState.WaitingForPlayerTurn 
                && _battle.Player.CurrentPlayerMode == BattlePlayerController.PlayerActionMode.PlacingLand)
            {
                bool isPlaced = _land.ReplaceEmptyLand(_battle.Player.CurrentlySelectedLand);
                if (isPlaced) _battle.Player.DecreaseAvailableCardsCount();
            }
        }
    }
}