


namespace Tenacity.Battles.Data
{
    public enum BattleState { Start, WaitingForEnemyTurn, WaitingForPlayerTurn, Won, Lost }
    public enum PlayerActionMode { None, PlacingLand, PlacingCard, MovingCreature }
    public enum EnemyDecisionMode { Move, Attack, PlaceLand, EndTurn, PlayCard}
}