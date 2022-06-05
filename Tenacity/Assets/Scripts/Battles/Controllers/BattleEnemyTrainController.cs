using System.Collections;
using Tenacity.Battles.Boards;
using Tenacity.Battles.Data;
using Tenacity.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Tenacity.Battles.Controllers
{
    public class BattleEnemyTrainController : MonoBehaviour
    {

        [Header("Battle objects")]
        [SerializeField] private Board _board;
        [SerializeField] private BattleEnemyController _player;
        [SerializeField] private BattleEnemyController _enemy;
        [Header("UI objects")]
        [SerializeField] private ImageMapper _turnState;
        [SerializeField] private Button _endTurnButton;
        [Header("The floats")]
        [SerializeField] private float _waitNextTurnTime = 0.5f;


        private readonly string[] _turnStateText = { "You", "Enemy" };


        public bool IsGameOver { get; private set; }
        public static BattleEnemyTrainController Instance;
        public BattleEnemyController Enemy { get => _enemy; }
        public BattleEnemyController Player { get => _player; }
        public BattleState CurrentBattleState { get; private set; }


        private void Awake() => Instance = this;


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
            _turnState.Value = (int)CurrentBattleState;
        }

        private void InitGame()
        {
            _player.Init(_board.LandCells[0]);
            _enemy.Init(_board.LandCells[^1]);
        }

        private IEnumerator PlayerTurn(float waitTime)
        {
            CurrentBattleState = BattleState.WaitingForPlayerTurn;
            _turnState.Value = (int)CurrentBattleState;

            yield return new WaitForSeconds(waitTime);
            yield return _player.MakeMove(waitTime);
            yield return new WaitForSeconds(waitTime);

            yield return EnemyTurn(waitTime);
        }

        private void ManageGameState()
        {
            if (_player.IsGameOver)
            {
                CurrentBattleState = BattleState.Lost;
                EndGame();
                StopAllCoroutines();
                IsGameOver = true;
            }
            else if (_enemy.IsGameOver)
            {
                CurrentBattleState = BattleState.Won;
                EndGame();
                StopAllCoroutines();
                IsGameOver = true;
            }
        }

        private bool IsPlayerGoesFirst()
        {
            return (Random.value > 0.5f);
        }

        private IEnumerator EnemyTurn(float waitTime)
        {
            CurrentBattleState = BattleState.WaitingForEnemyTurn;
            _turnState.Value = (int)CurrentBattleState;

            yield return new WaitForSeconds(waitTime);
            yield return _enemy.MakeMove(waitTime);
            yield return new WaitForSeconds(waitTime);

            yield return PlayerTurn(waitTime);
        }

        private IEnumerator StartBattle(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            CurrentBattleState = BattleState.Start;
            InitGame();

            if (IsPlayerGoesFirst()) StartCoroutine(PlayerTurn(waitTime));
            else StartCoroutine(EnemyTurn(waitTime));

            yield return new WaitForSeconds(waitTime);
        }
    }
}