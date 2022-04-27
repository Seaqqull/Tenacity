using System.Collections;
using System.Collections.Generic;
using Tenacity.Battle;
using UnityEngine;

namespace Tenacity.Lands
{
    public class LandCellController : MonoBehaviour
    {
        private Land _land;
        private BattleManager _battle;


        private void Start()
        {
            _land = GetComponent<Land>();
            _battle = transform.GetComponentInParent<BattleManager>();
        }

        public void HighlightNeighbors(LandType selectedType, bool highlighted)
        {
            if (!enabled) return;
            
            _land.OutlineLand(highlighted);
            foreach (Land neighbor in _land.NeighborLands)
            {
                if (neighbor.Type.HasFlag(selectedType) && neighbor.IsAvailableForCards)
                {
                    neighbor.OutlineLand(highlighted);
                }
            }
        }


        public void OnMouseDown()
        {
            if (_battle.CurrentBattleState == BattleManager.BattleState.WaitingForPlayerTurn)
            {
                BattlePlayerController player = _battle.Player;

                switch (player.CurrentPlayerMode){

                    case BattlePlayerController.PlayerActionMode.None | BattlePlayerController.PlayerActionMode.MovingCreature:
                        return;

                    case BattlePlayerController.PlayerActionMode.PlacingLand:

                        if (_land.Type != LandType.None) return;

                        bool isPlaced = _land.ReplaceLand(player.LandDeckInputController.CurrentlySelectedLand);
                        if (isPlaced) player.LandDeckInputController.DecreaseAvailableLandCardsCount();

                        return;

                    case BattlePlayerController.PlayerActionMode.PlacingCard:
                        if (_land.Type == LandType.None) return;

                        bool isCardPlaced = player.PlaceDeckCardOnLand(_land);
                        if (isCardPlaced) player.CurrentPlayerMode = BattlePlayerController.PlayerActionMode.None;

                        return;
                }
            }
        }
    }
}