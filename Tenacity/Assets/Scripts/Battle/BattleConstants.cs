using System.Collections;
using Tenacity.Lands;
using UnityEngine;

namespace Tenacity.Battle
{
    public static class BattleConstants
    {
        public static class LandConstants
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

        public const int ROUND_MANA = 2;
    }
}