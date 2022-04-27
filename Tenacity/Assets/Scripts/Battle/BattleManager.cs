using System.Collections;
using Tenacity.Boards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tenacity.Battle
{
    public class BattleManager : MonoBehaviour
    {
        public enum BattleState
        {
            Start,
            WaitingForEnemyTurn,
            WaitingForPlayerTurn,
            Won,
            Lost
        }

        
        [Header("Battle objects")]
        [SerializeField] private Board _board;
        [SerializeField] private BattleEnemyController _enemy;
        [SerializeField] private BattlePlayerController _player;


        [Header("UI objects")]
        [SerializeField] private TextMeshProUGUI _turnStateTextField;
        [SerializeField] private Button _endTurnButton;

        [Header("The floats")]
        [SerializeField] private float _waitNextTurnTime = 0.5f;



        public BattleEnemyController Enemy { get => _enemy; }
        public BattlePlayerController Player { get => _player; }
        public BattleState CurrentBattleState { get; private set; }


        private readonly string[] _turnStateText = { "You", "Enemy" };


        private void Start()
        {
            if (_enemy == null || _player == null) return;

            _endTurnButton.enabled = false;
            StartCoroutine(StartBattle(_waitNextTurnTime));
        }

        private void Update()
        {
            ManageGameState();
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
            _player.SetActiveMode(false);
            CurrentBattleState = BattleState.WaitingForEnemyTurn;
            _turnStateTextField.text = _turnStateText[1];

            yield return new WaitForSeconds(waitTime);
            yield return _enemy.MakeMove(_board.LandCells, waitTime);
            yield return new WaitForSeconds(waitTime);

            PlayerTurn();
        }
        
        private void PlayerTurn()
        {
            _player.SetActiveMode(true);
            CurrentBattleState = BattleState.WaitingForPlayerTurn;
            _turnStateTextField.text = _turnStateText[0];

            _endTurnButton.enabled = true;
        }

        public void OnEndTurnButton()
        {
            if (_player.CurrentPlayerMode != BattlePlayerController.PlayerActionMode.None) return;
            
            _endTurnButton.enabled = false;
            StartCoroutine(EnemyTurn(_waitNextTurnTime));
        }

        private bool IsPlayerGoesFirst()
        {
            return (Random.value > 0.5f);
        }

        private void ManageGameState()
        {
            if (_player.IsGameOver)
            {
                CurrentBattleState = BattleState.Lost;
                EndGame();
                StopAllCoroutines();
            }
            else if (_enemy.IsGameOver)
            {
                CurrentBattleState = BattleState.Won;
                EndGame();
                StopAllCoroutines();
            }
        }

        private void EndGame()
        {
            _turnStateTextField.text = "YOU " + CurrentBattleState;
        }
    }
}
