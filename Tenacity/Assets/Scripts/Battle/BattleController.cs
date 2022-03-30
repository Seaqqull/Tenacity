using System.Collections;
using System.Collections.Generic;
using Tenacity.Cards;
using Tenacity.Draggable;
using Tenacity.Lands;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tenacity.Battle
{
    public enum BattleState
    {
        START,
        WON,
        LOST
    }

    public class BattleController : MonoBehaviour
    {
        [SerializeField] private Board board;
        [SerializeField] private BattleEnemyAIController enemy;
        [SerializeField] private BattlePlayerController player;

        [SerializeField] private TextMeshProUGUI turnStateUI;
        [SerializeField] private Button endTurnButton;

        [SerializeField] private float waitNextTurnTime = 0.5f;

        private string[] _turnStateText = { "You", "Enemy" };
        private BattleState _battleState;

        private void Start()
        {
            if (enemy == null || player == null) return;

            player.SelectCardMode(false);
            endTurnButton.enabled = false;
            StartCoroutine(StartBattle(waitNextTurnTime));
        }

        private IEnumerator StartBattle(float waitTime)
        {
            _battleState = BattleState.START;
            yield return new WaitForSeconds(waitTime);
            if (IsPlayerGoesFirst()) PlayerTurn();
            else  StartCoroutine(EnemyTurn(waitTime));
            
        }
        
        private IEnumerator EnemyTurn(float waitTime)
        {
            turnStateUI.text = _turnStateText[1];
            yield return new WaitForSeconds(1.0f);
            enemy.PlaceLandOnBoard(board.LandCells, waitTime);
            yield return new WaitForSeconds(1.0f);
            enemy.PlaceCardIntoLand(board.LandCells, waitTime);
            yield return new WaitForSeconds(waitTime);

            if (enemy.IsGameOver())
            {
                _battleState = BattleState.WON;
                EndGame();
            }
            else
            {
                PlayerTurn();
            }
        }
        
        private void PlayerTurn()
        {
            if (player.IsGameOver())
            {
                _battleState = BattleState.LOST;
                EndGame();
                return;
            }
            turnStateUI.text = _turnStateText[0];
            player.SelectCardMode(true);
            endTurnButton.enabled = true;
        }

        public void OnEndTurnButton()
        {
            player.SelectCardMode(false);
            endTurnButton.enabled = false;
            StartCoroutine(EnemyTurn(waitNextTurnTime));
        }

        private bool IsPlayerGoesFirst()
        {
            return (Random.value > 0.5f);
        }

        private void EndGame()
        {
            turnStateUI.text = "YOU " + _battleState;
        }
    }
}
