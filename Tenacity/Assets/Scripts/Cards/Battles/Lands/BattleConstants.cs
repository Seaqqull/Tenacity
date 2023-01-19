


namespace Tenacity.Battles.Lands
{
    public static class BattleConstants
    {
        public const int ROUND_MANA = 2;


        public static class GameStateWeights
        {
            public const int MinionAttackWeight = 4;
            public const int MinionHealthWeight = 4;
            public const int MinionRangeWeight = 4;
            public const int MinionMovementWeight = 4;

            public const int HeroAttackWeight = 5;
            public const int HeroHealthWeight = 5;
            public const int HeroRangeWeight = 5;
            public const int HeroMovemenWeight = 5;
        }

        public static class GameStateRatings
        {
            public const int MinionMovedRating = 5;
            public const int MinionAttackRating = 5;
            public const int MinionKillerRating = 10;
            public const int MinionHasEnemyInRange = 5;
            public const int MinionDistanceToEnemyPerTile = 5;
            public const int MinionHasEnemyHeroInRangeRating = 100;
            public const int HandCardWeightPerHandCard = 4;
            public const int HeroIsDeadRating = 1000000;
        }
    }
}