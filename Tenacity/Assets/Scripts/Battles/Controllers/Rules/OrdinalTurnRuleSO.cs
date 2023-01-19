using Tenacity.Battles.Views.Creatures;
using Tenacity.Battles.Lands.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Tenacity.Battles.Controllers.Rules
{
    [CreateAssetMenu(fileName = "OrdinalTurnRule", menuName = "Battles/OrdinalTurnRule")]
    public class OrdinalTurnRuleSO : TurnRuleSO
    {
        [SerializeField] private int _ordinalGroundsToPlace;
        [SerializeField] private int _specialGroundsToPlace;
        // [SerializeField] private int _actionsPerCreature;
        // [SerializeField] private int _creaturesToPlace;
        // [SerializeField] private int _creaturesToMove;
        [Header("Read-only")]
        [SerializeField] private int _ordinalGroundsPlaced;
        [SerializeField] private int _specialGroundsPlaced;
        [SerializeField] private List<CreatureView> _usedCreatures = new List<CreatureView>();
        // [SerializeField] private int _creaturesPlaced;
        // [SerializeField] private int _creaturesMoved;


        public override bool IsTurnSealed()
        {
            return false;
        }

        
        public override void ResetRestrictions()
        {
            _ordinalGroundsPlaced = 0;
            _specialGroundsPlaced = 0;
            _usedCreatures.Clear();
        }
        
        public override bool DoMove(TurnMoveType moveType, TurnContext context)
        {
            if (!IsMoveAvailable(moveType, context)) return false;
            
            switch (moveType)
            {
                case TurnMoveType.PlaceLand:
                    var groundData = context as PlaceLandContext;
                    if (groundData!.Land == LandType.Neutral)
                        _ordinalGroundsPlaced++;
                    else
                        _specialGroundsPlaced++;
                    return true;
                case TurnMoveType.PlaceCreature:
                    return true;
                case TurnMoveType.MoveCreature:
                    var creatureData = context as MoveCreatureContext;
                    _usedCreatures.Add(creatureData!.Creature);
                    return true;
                default:
                    Debug.LogError($"[OrdinalTurnRule] Error: move of the given type({moveType}) was not found.");
                    return false;
            }
        }

        public override bool IsMoveAvailable(TurnMoveType moveType, TurnContext context)
        {
            switch (moveType)
            {
                case TurnMoveType.PlaceLand:
                    var groundData = context as PlaceLandContext;
                    return (groundData != null) && ((groundData.Land == LandType.Neutral) ? 
                        (_ordinalGroundsPlaced < _ordinalGroundsToPlace) : 
                        (_specialGroundsPlaced < _specialGroundsToPlace));
                case TurnMoveType.PlaceCreature:
                    return true;
                case TurnMoveType.MoveCreature:
                    var creatureData = context as MoveCreatureContext;
                    return (creatureData != null) && _usedCreatures.All(creature => creature != creatureData.Creature);
                default:
                    Debug.LogError($"[OrdinalTurnRule] Error: move of the given type({moveType}) was not found.");
                    return false;
            }
        }
    }
}