using Tenacity.Battles.Data.Field;
using Tenacity.Battles.Lands.Data;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Tenacity.Battles.Data
{
    public enum NewBattleState { None, Initialization, TopTurn, BottomTurn, End }
    public enum InteractionState { None, Active, Blocked }
    
    public interface IInteractionView
    {
        public InteractionState State { get; set; }
    }

    public interface IBoard
    {
        // public Player
        public IReadOnlyList<IPlayer> Players { get; }
        // public TurnRuleSO Rule { get; }
        public IEnumerable<ICell> AllCells { get; }
        public ICell[][] Field { get; }
        public MonoBehaviour Mono { get; }

        public void EndTurn(int playerId);
        public void SetUI(bool manualInput);
        public TeamType OpponentTeam(TeamType team);

        
        public void SelectCell(Vector2Int fieldPosition);
        public bool SelectLand(int playerId, LandType land);
        public bool SelectCard(int playerId, int handCardIndex);

        public void DeselectCard(int playerId);
        public int DistanceBetweenCells(ICell fromCell, ICell toCell);
        public int CountOfCards(Func<ICell, bool> selector, TeamType team);
        public bool IsEnoughCards(Func<ICell, bool> selector, TeamType team, int count);
        
        public int CountOfCards(ICell cell, Func<ICell, bool> selector, int distance = -1);

        public void ResetCellSelection();
        public void ResetSelectParameters(bool card = false, bool land = false, bool creature = false);
    }

    // public interface IAiBoard : IBoard
    // {
    // }

    public interface ICell
    {
        public InteractionState SelectionState { get; }
        public Vector2Int FieldPosition { get; }
        public ICreatureData CreatureData { get; }
        public ICellState State { get; }
        // public CreatureView
    }

    public interface ICreatureData
    {
        public TeamType Team { get; }
        public int Priority { get; }
        public int Power { get; }
        public int Range { get; }
        public int Life { get; }
    }
}