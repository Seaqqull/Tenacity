using Tenacity.Battles.Lands.Data;


namespace Tenacity.Battles
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