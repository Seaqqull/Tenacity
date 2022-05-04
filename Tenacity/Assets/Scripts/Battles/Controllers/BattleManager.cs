using Tenacity.Battles.Boards;
using Tenacity.Battles.Lands;
using Tenacity.Battles.Data;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace Tenacity.Battles.Controllers
{
    public class BattleManager : MonoBehaviour
    {
        [Header("Battle objects")]
        [SerializeField] private Board _board;
        [SerializeField] private BattleEnemyController _enemy;
        [SerializeField] private BattlePlayerController _player;
        [Header("UI objects")]
        [SerializeField] private TextMeshProUGUI _turnStateTextField;
        [SerializeField] private Button _endTurnButton;
        [Header("The floats")]
        [SerializeField] private float _waitNextTurnTime = 0.5f;


        private readonly string[] _turnStateText = { "You", "Enemy" };
        public BattleState CurrentBattleState { get; private set; }
        public BattlePlayerController Player { get => _player; }
        public BattleEnemyController Enemy { get => _enemy; }

        

        private void Start()
        {
            if ((_enemy == null) || (_player == null)) return;

            _endTurnButton.enabled = false;
            StartCoroutine(StartBattle(_waitNextTurnTime));
        }

        private void Update()
        {
            if (CurrentBattleState == BattleState.Start) return;
            ManageGameState();
        }


        private void EndGame()
        {
            _turnStateTextField.text = "YOU " + CurrentBattleState;
        }
        
        private void InitGame()
        {
            _player.Init(_board.LandCells[0]);
            _enemy.Init(_board.LandCells[^1]);

        }

        private void PlayerTurn()
        {
            _player.SetActiveMode(true);
            _endTurnButton.enabled = true;
            
            _turnStateTextField.text = _turnStateText[0];
            CurrentBattleState = BattleState.WaitingForPlayerTurn;
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

        private bool IsPlayerGoesFirst()
        {
            return (Random.value > 0.5f);
        }
        
        private IEnumerator EnemyTurn(float waitTime)
        {
            _player.SetActiveMode(false);
            CurrentBattleState = BattleState.WaitingForEnemyTurn;
            _turnStateTextField.text = _turnStateText[1];

            yield return new WaitForSeconds(waitTime);
            yield return _enemy.MakeMove(waitTime);
            yield return new WaitForSeconds(waitTime);

            PlayerTurn();
        }
        
        private IEnumerator StartBattle(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            
            CurrentBattleState = BattleState.Start;
            InitGame();

            if (IsPlayerGoesFirst()) PlayerTurn();
            else  StartCoroutine(EnemyTurn(waitTime));
            
            yield return new WaitForSeconds(waitTime);
        }
        
        
        public void OnEndTurnButton()
        {
            if (_player.CurrentPlayerMode != PlayerActionMode.None) return;
            
            _endTurnButton.enabled = false;
            StartCoroutine(EnemyTurn(_waitNextTurnTime));
        }
    }
}
