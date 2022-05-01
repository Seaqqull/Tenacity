﻿using Tenacity.Battles.Lands.Data;
using Tenacity.Battles.Data;
using UnityEngine;


namespace Tenacity.Battles.Lands
{
    public class LandCellController : MonoBehaviour
    {
        private BattleManager _battle;
        private Land _land;


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
            if (!enabled) return;
            if (_battle.CurrentBattleState != BattleState.WaitingForPlayerTurn) return;
            
            BattlePlayerController player = _battle.Player;
            switch (player.CurrentPlayerMode)
            {

                case PlayerActionMode.None | PlayerActionMode.MovingCreature:
                    return;
                case PlayerActionMode.PlacingLand:
                    if (_land.Type != LandType.None) return;

                    bool isPlaced = _land.ReplaceLand(player.LandDeckInputController.CurrentlySelectedLand);
                    if (isPlaced) 
                        player.LandDeckInputController.DecreaseAvailableLandCardsCount();
                    return;
                case PlayerActionMode.PlacingCard:
                    if (_land.Type == LandType.None) return;

                    bool isCardPlaced = player.PlaceDeckCardOnLand(_land);
                    if (isCardPlaced) 
                        player.CurrentPlayerMode = PlayerActionMode.None;
                    return;
            }
        }
    }
}