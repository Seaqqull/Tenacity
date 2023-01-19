using UnityEngine;


namespace Tenacity.Battles.Controllers.Rules
{
    public abstract class TurnRuleSO : ScriptableObject
    {
        public abstract bool IsTurnSealed();
        public abstract void ResetRestrictions();
        public abstract bool DoMove(TurnMoveType moveType, TurnContext context);
        public abstract bool IsMoveAvailable(TurnMoveType moveType, TurnContext context);
    }
}