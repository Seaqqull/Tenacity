using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tenacity.Cards;
using Tenacity.Draggable;
using Tenacity.Lands;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tenacity.Battle
{
    public class BattleController : MonoBehaviour
    {
        public enum BattleState
        {
            Start,
            WaitingForEnemyTurn,
            WaitingForPlayerTurn,
            Won,
            Lost
        }

        [SerializeField] private Board _board;
        [SerializeField] private BattleEnemyController _enemy;
        [SerializeField] private BattlePlayerController _player;
        [SerializeField] private TextMeshProUGUI _turnStateUI;
        [SerializeField] private Button _endTurnButton;
        [SerializeField] private float _waitNextTurnTime = 0.5f;

        public BattlePlayerController Player { get => _player; }
        public BattleEnemyController Enemy { get => _enemy; }
        public BattleState CurrentBattleState { get; private set; }

        private readonly string[] _turnStateText = { "You", "Enemy" };


        private void Start()
        {
            if (_enemy == null || _player == null) return;

            _endTurnButton.enabled = false;
            StartCoroutine(StartBattle(_waitNextTurnTime));
        }

        private IEnumerator StartBattle(float waitTime)
        {
            CurrentBattleState = BattleState.Start;
            if (IsPlayerGoesFirst()) PlayerTurn();
            else  StartCoroutine(EnemyTurn(waitTime));
            yield return new WaitForSeconds(waitTime);

        }
        
        private IEnumerator EnemyTurn(float waitTime)
        {
            _player.SelectCardMode(false);
            CurrentBattleState = BattleState.WaitingForEnemyTurn;
            _turnStateUI.text = _turnStateText[1];

            yield return new WaitForSeconds(1.0f);
            yield return _enemy.MakeMove(_board.LandCells, waitTime);
            yield return new WaitForSeconds(waitTime);

            if(!IsGameover()) PlayerTurn();
        }
        
        private void PlayerTurn()
        {
            _player.SelectCardMode(true);
            CurrentBattleState = BattleState.WaitingForPlayerTurn;
            _turnStateUI.text = _turnStateText[0];
            if (!IsGameover()) _endTurnButton.enabled = true;
        }

        public void OnEndTurnButton()
        {
            _endTurnButton.enabled = false;
            StartCoroutine(EnemyTurn(_waitNextTurnTime));
        }

        private bool IsPlayerGoesFirst()
        {
            return (Random.value > 0.5f);
        }

        private bool IsGameover()
        {
            if (_player.IsGameOver)
            {
                CurrentBattleState = BattleState.Lost;
                EndGame();
                StopAllCoroutines();
                return true;
            } else if (_enemy.IsGameOver)
            {
                CurrentBattleState = BattleState.Won;
                EndGame();
                StopAllCoroutines();
                return true;
            }
            return false;
        }
        private void EndGame()
        {
            _turnStateUI.text = "YOU " + CurrentBattleState;
        }
    }
}
