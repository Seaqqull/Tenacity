using System;
using Tenacity.Battles.Lands.Data;


namespace Tenacity.Battles.Data.Field
{
    [Flags]
    public enum TeamType { Any = Bottom | Top, Bottom = 1, Top = 2 }
    public enum CellType { None, Player, Ground, Creature }
    public enum FieldCreationType { Empty, Ground, Player }

    public interface ICellState
    {
        public LandType LandType { get; }
        public bool IsImmutable { get; } // some property to check from ?
        public TeamType Team { get; }
        public CellType Type { get; }
    }
}