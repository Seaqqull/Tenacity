using Tenacity.Battles.Views.Creatures;
using Tenacity.Battles.Lands.Data;
using Tenacity.Cards;


namespace Tenacity.Battles.Controllers.Rules
{
    public enum  TurnMoveType { Nothing, PlaceLand, PlaceCreature, MoveCreature }

    public class TurnContext { }
    

    public class PlaceLandContext : TurnContext
    {
        public LandType Land { get; }

        
        public PlaceLandContext(LandType land)
        {
            Land = land;
        }
    }
    
    public class PlaceCreatureContext : TurnContext
    {
        public CardSO Card { get; }

        
        public PlaceCreatureContext(CardSO card)
        {
            Card = card;
        }
    }
    
    public class MoveCreatureContext : TurnContext
    {
        public CreatureView Creature { get; }

        
        public MoveCreatureContext(CreatureView creature)
        {
            Creature = creature;
        }
    }
}