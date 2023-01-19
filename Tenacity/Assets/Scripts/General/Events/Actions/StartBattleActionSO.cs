using Tenacity.Battles.Controllers;
using Tenacity.Battles.Data.Field;
using Tenacity.Cards.Cards.Data;
using Tenacity.Battles.Players;
using Tenacity.Utility.Methods;
using Tenacity.Battles.Data;
using System.Linq;
using Tenacity.Managers;
using UnityEngine;
using UnityEngine.Events;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Battles/StartBattle", fileName = "StartBattle", order = 0)]
    public class StartBattleActionSO : ActionSO<bool>
    {
        #region Constants
        private const char FIELD_NOTHING = ' ';
        private const char FIELD_PLAYER = '_';
        private const char FIELD_GROUND = '*';
        #endregion

        [Header("Actions")] 
        [SerializeField] private UnityEvent _onWin;
        [SerializeField] private UnityEvent _onLose;
        [Header("Field")] 
        [SerializeField] [TextArea] private string _field;
        [Header("Player")] 
        [SerializeField] private CardDeckSO _playerDeck;
        [SerializeField] private PlayerDataSO _playerData;
        [Header("Enemy")] 
        [SerializeField] private AIWeightsSO _aiWeights;
        [SerializeField] private CardDeckSO _opponentDeck;
        [SerializeField] private PlayerDataSO _opponentData;

        
        private PlayerData ParseData(PlayerDataSO data)
        {
            return new PlayerData()
            {
                Health = data.Health,
                MaxHealth = data.MaxHealth,

                Mana = data.Mana,

                HandSize = data.HandSize,
                Name = data.Name,
                Seed = new System.Random(data.Name.GetHashCode() + 
                                         (int)Hasher.CurrentTimeMillis()).Next()
            };
        }
        

        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return true;
        }

        public void BackupBattleData()
        {
            var opponentData = ParseData(_opponentData);
            var playerData = ParseData(_playerData);
            opponentData.Deck = new PlayerCards(_opponentDeck.Cards);
            playerData.Deck = new PlayerCards(_playerDeck.Cards);

            var playerPosition = PlayerManager.Instance.transform.position;
            var levelName = SceneManager.Instance.LevelName;
            var levelIndex = SceneManager.Instance.LevelIndex;


            BattleManager.Instance.BackupBattleData(new BattleData()
            {
                AiBehabiour = _aiWeights,
                Field = _field.Split("\n").Select(fieldRow =>
                {
                    return fieldRow.Select(rowCell =>
                        (rowCell == FIELD_PLAYER) ? FieldCreationType.Player : 
                        (rowCell == FIELD_GROUND) ? FieldCreationType.Ground : FieldCreationType.Empty).ToArray();
                }).ToArray(),
                
                Players = new Battles.Players.Player[]
                {
                    _playerData.IsAi ? new AiPlayer(playerData, 0, TeamType.Bottom, _aiWeights) : 
                        new ManualPlayer(playerData, 0, TeamType.Bottom),
                    _opponentData.IsAi ? new AiPlayer(opponentData, 1, TeamType.Top, _aiWeights) :
                        new ManualPlayer(opponentData, 1, TeamType.Top) 
                },
                
                OnFinishGame = (isWin) =>
                {
                    StorageManager.Instance.UpdatePlayerPosition(playerPosition);
                    SceneManager.Instance.LoadLevel(levelIndex, levelName, () =>
                    {
                        if (isWin)
                            _onWin?.Invoke();
                        else
                            _onLose?.Invoke();
                    });
                }
            });
        }
    }
}