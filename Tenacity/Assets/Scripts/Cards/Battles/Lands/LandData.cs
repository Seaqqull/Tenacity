using System;


namespace Tenacity.Battles.Lands.Data
{
    [Flags] 
    public enum LandType
    {
        Neutral =  Ice | Water | Fire | Earth,
        None = 1,
        Ice =   2,
        Water = 4,
        Fire =  8,
        Earth = 16
    }
}