using Tenacity.Battles.Controllers.Rules;
using Tenacity.Battles.Views.Creatures;
using Tenacity.Battles.Lands.Data;
using Tenacity.Battles.Data;
using Tenacity.Cards;
using UnityEngine;


namespace Tenacity.Battles.Players
{
    public abstract class AiEvaluation
    {
        public abstract TurnMoveType Type { get; }

        
        public abstract TurnContext GetTurnContext();
    }

    public class LandAiEvaluation : AiEvaluation
    {
        public override TurnMoveType Type => TurnMoveType.Nothing;
        public ICell Land { get; }

        
        public LandAiEvaluation(ICell land)
        {
            Land = land;
        }


        public override TurnContext GetTurnContext()
        {
            return new TurnContext();
        }
    }
    
    public class PlaceLandAiEvaluation : AiEvaluation
    {
        public override TurnMoveType Type => TurnMoveType.PlaceLand;
        public Vector2Int CellToPlace { get; }

        
        public PlaceLandAiEvaluation(Vector2Int cellToPlace)
        {
            CellToPlace = cellToPlace;
        }


        public override TurnContext GetTurnContext()
        {
            return new TurnContext();
        }
    }
    
    public class PlaceCreatureAiEvaluation : AiEvaluation
    {
        public override TurnMoveType Type => TurnMoveType.PlaceCreature;
        public Vector2Int CellToPlace { get; }
        public LandType LandType { get; }
        public CardSO Card { get; }


        public PlaceCreatureAiEvaluation(CardSO card, Vector2Int cellToPlace, LandType landType = LandType.None)
        {
            CellToPlace = cellToPlace;
            LandType = landType;
            Card = card;
        }
        
        
        public override TurnContext GetTurnContext()
        {
            return new PlaceCreatureContext(Card);
        }
    }
    
    public class MoveCreatureAiEvaluation : AiEvaluation
    {
        public override TurnMoveType Type => TurnMoveType.MoveCreature;
        public Vector2Int CreaturePosition { get; }
        public Vector2Int CellToMove { get; }


        public MoveCreatureAiEvaluation(Vector2Int creaturePosition, Vector2Int cellToMove)
        {
            CellToMove = cellToMove;
            CreaturePosition = creaturePosition;
        }
        
        
        public override TurnContext GetTurnContext()
        {
            return new TurnContext();
        }
    }
}