using System.Linq;
using Tenacity.Battles.Controllers;
using Tenacity.Battles.Data;
using Tenacity.Battles.Data.Field;
using Tenacity.Battles.Players;
using Tenacity.Cards.Cards.Data;
using Tenacity.Managers;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/UpdatePlayerDeckForBattle", fileName = "UpdatePlayerDeckForBattle", order = 0)]
    public class UpdatePlayerDeckForBattleSO : ActionSO<bool>
    {
        #region Constants
        private const int PLAYER_INDEX = 0;
        #endregion
        
        [SerializeField] private CardDeckSO _playerDeck;
        
        
        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return true;
        }


        public void UpdatePlayerDeck()
        {
            var battleData = BattleManager.Instance.GetData();
            if (battleData == null) return;

            var player = battleData.Players.ElementAt(PLAYER_INDEX) as Battles.Players.Player;
            (player as IPlayerData).UpdateDeck(new PlayerCards(_playerDeck.Cards));
            
            BattleManager.Instance.BackupBattleData(battleData);
        }
    }
}