using System.Collections;
using Tenacity.Lands;
using UnityEngine;

namespace Tenacity.Battle
{
    public static class BattleRules
    {
        public static class LandRules
        {
            public static int GetLandCellsCount(LandType landType)
            {
                return landType switch
                {
                    LandType.Neutral => 2,
                    _ => 1,
                };
            }
        }

        public static readonly int ROUND_MANA = 2;
    }
}