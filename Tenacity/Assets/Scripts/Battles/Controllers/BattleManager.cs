using Tenacity.Battles.Data.Field;
using System.Collections.Generic;
using Tenacity.Battles.Data;
using Tenacity.Base;
using UnityEngine;
using System;
using Tenacity.Battles.Players;


namespace Tenacity.Battles.Controllers
{
    public class BattleData
    {
        public FieldCreationType[][] Field { get; set; } = Array.Empty<FieldCreationType[]>();
        public IEnumerable<IPlayer> Players { get; set; } = Array.Empty<IPlayer>();
        public AIWeightsSO AiBehabiour { get; set; }

        public Action<bool> OnFinishGame { get; set; }
    }

    
    public class BattleManager : SingleBehaviour<BattleManager>
    {
        private BattleData _data;


        public void ClearBattleData()
        {
            _data = null;
        }

        public BattleData GetData()
        {
            if (_data == null)
            {
                Debug.LogError("[BattleManager] Error: Battle data is empty.");
                return new BattleData();
            }
            
            return _data;
        }
        
        public void BackupBattleData(BattleData data)
        {
            _data = data;
        }
    }
}
