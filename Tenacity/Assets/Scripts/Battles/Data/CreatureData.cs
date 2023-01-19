using System;
using Tenacity.Battles.Data.Field;
using Tenacity.Battles.Lands.Data;


namespace Tenacity.Battles.Data
{
    public struct CreatureData : ICreatureData
    {
        public Action<int> OnHealthUpdate { get; set; }
        public Action OnDeath { get; set; }
        public TeamType Team { get; set; }
        public LandType Type { get; set; }
        public int Priority { get; set; }
        public int Power { get; set; }
        public int Range { get; set; }
        public int Life { get; set; }
    }

    public struct CardData
    {
        
    }
}