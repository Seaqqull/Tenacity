using Tenacity.Battles.Lands.Data;


namespace Tenacity.Battles.Data.Field
{
    public class EmptyCell : ICellState
    {
        public LandType LandType => LandType.None;
        public CellType Type => CellType.None;
        public TeamType Team => TeamType.Any;
        public bool IsImmutable => false;
    }
    
    public sealed class PlayerCell : ICellState
    {
        public LandType LandType => LandType.Neutral; // None?
        public bool IsImmutable => true;
        public TeamType Team { get; protected set; }
        public CellType Type { get; protected set; }

        
        public PlayerCell(TeamType team)
        {
            Type = CellType.Player;
            Team = team;
        }
    }

    public class LandCell : ICellState
    {
        public LandType LandType { get; protected set; }
        public bool IsImmutable => false;
        public CellType Type { get; protected set; }
        public TeamType Team { get; protected set; }



        public LandCell(TeamType team, LandType land)
        {
            Type = CellType.Ground;
            LandType = land;
            Team = team;
        }

        public LandCell(CreatureCell creature)
        {
            LandType = creature.LandType;
            Team = creature.Team;
            Type = creature.Type;
        }
    }

    public sealed class CreatureCell : LandCell
    {
        public CreatureCell(LandCell land) : base(land.Team, land.LandType)
        {
            Type = CellType.Creature;
        }
    }
}