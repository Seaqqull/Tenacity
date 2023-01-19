using UnityEngine;


namespace Tenacity.Battles.Players
{
    [CreateAssetMenu(fileName = "StraightWeight", menuName = "Battles/AI/StraightWeight")]
    public class StraightWeights : AIWeightsSO
    {
        [field: SerializeField] public override int CreatureNearOpposingCreaturesRating { get; protected set; } = 10;
        [field: SerializeField] public override int DistanceToOpponentRating { get; protected set; } = 10;
        [field: SerializeField] public override int CreatureIsAtCellRating { get; protected set; } = 15;
        [field: SerializeField] public override int PlayerIsAtCellRating { get; protected set; } = 50;


        
        [field: SerializeField] public override int MinionMovedRating { get; protected set; } = 5;
        [field: SerializeField] public override int MinionAttackRating { get; protected set; } = 5;
        [field: SerializeField] public override int MinionKillerRating { get; protected set; } = 10;
        [field: SerializeField] public override int MinionHasEnemyInRange { get; protected set; } = 5;
        [field: SerializeField] public override int MinionDistanceToEnemyPerTile { get; protected set; } = 5;
        [field: SerializeField] public override int MinionHasEnemyHeroInRangeRating { get; protected set; } = 100;
        [field: SerializeField] public override int HandCardWeightPerHandCard { get; protected set; } = 4;
        [field: SerializeField] public override int HeroIsDeadRating { get; protected set; } = 1000000;
    }
}