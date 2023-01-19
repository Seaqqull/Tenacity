using UnityEngine;


namespace Tenacity.Battles.Players
{
    public abstract class AIWeightsSO : ScriptableObject
    {
        // Add weight for distance to player;
        public abstract int DistanceToOpponentRating { get; protected set; }
        public abstract int CreatureNearOpposingCreaturesRating { get; protected set; }
        public abstract int CreatureIsAtCellRating { get; protected set; }
        public abstract int PlayerIsAtCellRating { get; protected set; }
        
        public abstract int MinionMovedRating { get; protected set; }
        public abstract int MinionAttackRating { get; protected set; }
        public abstract int MinionKillerRating { get; protected set; }
        public abstract int MinionHasEnemyInRange { get; protected set; }
        public abstract int MinionDistanceToEnemyPerTile { get; protected set; }
        public abstract int MinionHasEnemyHeroInRangeRating { get; protected set; }
        public abstract int HandCardWeightPerHandCard { get; protected set; }
        public abstract int HeroIsDeadRating { get; protected set; }
    }
}