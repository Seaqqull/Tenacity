using System;


namespace Tenacity.Battles.Lands.Data
{
    [Flags] 
    public enum LandType
    {
        None = 0,
        Ice = 1 << 0,
        Water = 1 << 1,
        Fire = 1 << 2,
        Earth = 1 << 3,
        Neutral = ~0,
    }
}