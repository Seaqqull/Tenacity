using Tenacity.Battles.Controllers.Rules;
using Tenacity.Battles.Data.Field;
using Tenacity.Battles.Lands.Data;
using System.Collections.Generic;
using Tenacity.Battles.Data;
using Tenacity.Utility;
using Tenacity.Cards;
using System.Linq;
using UnityEngine;
using System;
using System.Collections;
using Tenacity.Utility.Methods;


namespace Tenacity.Battles.Players
{
    public sealed class AiPlayer : Player
    {
        private struct CellEvaluation
        {
            public float Priority;
            public AiEvaluation Action;

            public CellEvaluation(float priority, AiEvaluation action)
            {
                Priority = priority;
                Action = action;
            }
        }
        
        
        #region Constants
        private int DEFAULT_ATTACK_RANGE = 1;
        #endregion
        
        private Dictionary<TurnMoveType, PriorityQueue<AiEvaluation>> _allowedActions = new(); 
        private AIWeightsSO _weights;


        private TeamType _opposingTeam;
        private ICell _opposingPlayer;
        private IBoard _board;

        
        public AiPlayer(IPlayerData data, int id, TeamType team, AIWeightsSO weights) : base(data, id, team)
        {
            _weights = weights;
        }


        private void ClearActions()
        {
            foreach (var certainActions in _allowedActions)
                certainActions.Value.Clear();
        }

        private void AddAction(float priority, AiEvaluation evaluation)
        {
            if (!_allowedActions.TryGetValue(evaluation.Type, out var certainActions))
            {
                certainActions = new PriorityQueue<AiEvaluation>();
                _allowedActions.Add(evaluation.Type, certainActions);
            }
            certainActions.Enqueue(priority, evaluation);
        }

        private void InitializeData(IBoard board)
        {
            _board = board;
            _opposingTeam = _board.OpponentTeam(Team);
            _opposingPlayer = _board.Field.SelectMany(cellRow => cellRow.Select(cell => cell)).Single(cell =>
                (cell.State.Type == CellType.Player) && (cell.State.Team.HasFlag(_opposingTeam)));
        }
        
        private void EvaluateCards()
        {
            var handCards = Hand.Cards.ToList();
            var possibleLands = !_allowedActions.ContainsKey(TurnMoveType.Nothing) ? 
                Array.Empty<LandAiEvaluation>() :
                _allowedActions[TurnMoveType.Nothing].Where(action => 
                    (action.Object.Type == TurnMoveType.PlaceLand)
                ).Select(action => (action.Object as LandAiEvaluation));

            var playableCards = handCards.Where(card => 
                (card.CastingCost <= Mana) &&
                ((card.LandCost == 0) || 
                 (card.LandCost <= (possibleLands.Count(land => (land.Land.State.LandType == card.Land)) + 1)))).
                GroupBy(card => card.Name).
                Select(cardGroup => cardGroup.First());

            foreach (var card in playableCards)
            {
                var cardLandsPlaced = possibleLands.Count(land => (land.Land.State.LandType == card.Land));
                var evaluatedCells = Array.Empty<CellEvaluation>();
                var availableCells = Array.Empty<ICell>();
                
                if ((cardLandsPlaced != 0) && (card.LandCost <= cardLandsPlaced))
                {
                    _board.SelectCard(Id, handCards.IndexOf(card)); // Needed to make sure, we select only with [SelectionState = InteractionState.Active] 

                    availableCells = _board.AllCells.Where(cell => (cell.SelectionState == InteractionState.Active)).ToArray();
                    evaluatedCells = availableCells.Select(cell => EvaluateCell(_board, cell)).ToArray();
                    
                    _board.DeselectCard(Id); // Remove [SelectionState = InteractionState.Active] from cells
                }
                else if (card.LandCost <= (cardLandsPlaced + 1))
                {
                    _board.SelectLand(Id, card.Land); // Needed to make sure, we select only with [SelectionState = InteractionState.Active] 
                    
                    availableCells = _board.AllCells.Where(cell => (cell.SelectionState == InteractionState.Active)).ToArray();
                    evaluatedCells = _board.AllCells.
                        Where(cell => (cell.SelectionState == InteractionState.Active)).
                        Select(cell => EvaluateCell(_board, cell)).ToArray();
                    
                    _board.SelectLand(Id, LandType.None); // Remove [SelectionState = InteractionState.Active] from cells
                }

                for (int i = 0; i < availableCells.Length; i++)
                {
                    AddAction(
                        evaluatedCells[i].Priority, 
                        new PlaceCreatureAiEvaluation(
                            card, 
                            availableCells[i].FieldPosition, 
                            (card.LandCost <= (cardLandsPlaced + 1)) ? card.Land : LandType.None
                        ));
                }
            }
        }

        private void EvaluateLands()
        {
            if (_allowedActions.ContainsKey(TurnMoveType.Nothing))
                _allowedActions[TurnMoveType.Nothing].Clear();
            // Evaluate own [Ground] cells
            var evaluatedOwnCells = _board.AllCells.
                Where(cell => (cell.State.Type == CellType.Ground) && (cell.State.Team == Team))
                .Select(cell => EvaluateCell(_board, cell));

            // Evaluate near [Empty] cells
            _board.SelectLand(Id, LandType.Neutral); // Needed to make sure, we select only with [SelectionState = InteractionState.Active] 
            var evaluatedEmptyCells = _board.AllCells.
                Where(cell => (cell.SelectionState == InteractionState.Active)).
                Select(cell => EvaluateCell(_board, cell)).ToList();
            _board.SelectLand(Id, LandType.None); // Remove [SelectionState = InteractionState.Active] from cells

            var evaluatedCells = evaluatedOwnCells.Concat(evaluatedEmptyCells);
            foreach (var evaluatedCell in evaluatedCells)
                AddAction(evaluatedCell.Priority, evaluatedCell.Action);
        }
        
        private void EvaluateLandPlacement()
        {
            _board.SelectLand(Id, LandType.Neutral); // Needed to make sure, we select only with [SelectionState = InteractionState.Active]
            var evaluatedNeutralCells = _board.AllCells.
                Where(cell => (cell.SelectionState == InteractionState.Active)).
                Select(cell => EvaluateCell(_board, cell)).ToList();
            _board.SelectLand(Id, LandType.None); // Remove [SelectionState = InteractionState.Active] from cells
            
            _board.SelectLand(Id, LandType.Fire); // Needed to make sure, we select only with [SelectionState = InteractionState.Active] 
            var evaluatedSpecialCells = _board.AllCells.
                Where(cell => (cell.SelectionState == InteractionState.Active)).
                Select(cell => EvaluateCell(_board, cell)).ToList();
            _board.SelectLand(Id, LandType.None); // Remove [SelectionState = InteractionState.Active] from cells
            
            var evaluatedCells = evaluatedNeutralCells.Concat(evaluatedSpecialCells);
            foreach (var evaluatedCell in evaluatedCells)
                AddAction(evaluatedCell.Priority, new PlaceLandAiEvaluation((evaluatedCell.Action as LandAiEvaluation).Land.FieldPosition));
        }
        
        private void EvaluateCreatureMovement()
        {
            // Evaluate own [Ground] cells
            var creatureCells = _board.AllCells.
                Where(cell => (cell.State.Type == CellType.Creature) && (cell.CreatureData.Team == Team));

            // Evaluate possible steps for creature
            foreach (var creatureCell in creatureCells)
            {
                _board.SelectCell(creatureCell.FieldPosition);
                
                var availableToMoveCells = _board.AllCells.
                    Where(cell => (cell.SelectionState == InteractionState.Active)).ToList();
                var evaluatedAvailableCells = availableToMoveCells.
                    Select(cell => EvaluateCell(_board, cell)).ToList();
                _board.ResetCellSelection();

                for (int i = 0; i < availableToMoveCells.Count; i++)
                {
                    AddAction(
                        (evaluatedAvailableCells[i].Priority + creatureCell.CreatureData.Priority),
                        new MoveCreatureAiEvaluation(
                            creatureCell.FieldPosition,
                            availableToMoveCells[i].FieldPosition)
                    );
                }

                var creatureCellEvaluation = EvaluateCell(_board, creatureCell);
                AddAction(
                    (creatureCellEvaluation.Priority + creatureCell.CreatureData.Priority),
                    new MoveCreatureAiEvaluation( creatureCell.FieldPosition, creatureCell.FieldPosition)
                );
            }
        }
        
        

        private CellEvaluation EvaluateCell(IBoard board, ICell cell)
        { // if cell is creature => more 
            var nearEnemies = board.CountOfCards(cell,
                cell => (cell.State.Type == CellType.Creature) && (cell.CreatureData.Team == _opposingTeam), DEFAULT_ATTACK_RANGE);
            var enemiesInRangeRating = ((cell.State.Type == CellType.Creature) && (cell.CreatureData.Team == _opposingTeam)) ?
                _weights.CreatureIsAtCellRating : 
                ((cell.State.Type == CellType.Player) && (cell.CreatureData.Team == _opposingTeam)) ?
                    _weights.PlayerIsAtCellRating :
                    _weights.CreatureNearOpposingCreaturesRating * (nearEnemies / 6.0f);
            var distanceToOpposingPlayer = _weights.DistanceToOpponentRating / 
                                           Mathf.Sqrt(board.DistanceBetweenCells(cell, _opposingPlayer));

            return new CellEvaluation(enemiesInRangeRating + distanceToOpposingPlayer, new LandAiEvaluation(cell));
        }

        private CellEvaluation EvaluateCardPlacement(IBoard board, ICell cell, CardSO card, LandType land = LandType.None)
        {
            var movementRating = (float)card.Rating;
            if (cell != null)
                movementRating += EvaluateCell(board, cell).Priority;
            
            return new CellEvaluation(movementRating, new PlaceCreatureAiEvaluation(card, cell.FieldPosition, land));
        }

        private IEnumerator MakeTurn()
        {
            System.Random stepsShuffler = new System.Random(Seed +
                                                           (int) Hasher.CurrentTimeMillis());
            // Stages to determine best movements
            // 1. Evaluate lands for [Evaluate cards] movement
            // 2. Place cards on field, if possible
            // 3. Place remaining fields, if possible
            // 4. Analyze the best movement for the existing creatures => perform movement

            // --- 1: Evaluate lands for [Evaluate cards] movement
            // Evaluation
            EvaluateLands();
            
            
            // --- 2: Place cards on field, if possible
            // Evaluation 
            EvaluateCards();
            
            // Perform actions
            var movesToPlaceCard = !_allowedActions.ContainsKey(TurnMoveType.PlaceCreature) ? 
                Array.Empty<PlaceCreatureAiEvaluation>() : 
                _allowedActions[TurnMoveType.PlaceCreature].
                    OrderBy(step => stepsShuffler.Next()).
                    OrderByDescending(action => action.Priority).
                    Select(action => action.Object).
                    Select(action => action as PlaceCreatureAiEvaluation).ToArray();
            
            foreach (var action in movesToPlaceCard)
            {
                var handIndex = Hand.IndexOf(action.Card);
                if ((handIndex == -1) || (_board.Field[action.CellToPlace.x][action.CellToPlace.y].State.Type == CellType.Creature)) continue;

                if (action.LandType != LandType.None)
                {
                    _board.SelectLand(Id, action.LandType);
                    _board.SelectCell(action.CellToPlace);
                }

                _board.SelectCard(Id, handIndex);
                _board.SelectCell(action.CellToPlace);
            }
            yield return null;
            
            
            // --- 3: Place remaining fields, if possible
            // Evaluate
            EvaluateLandPlacement();
            
            // Perform actions
            var movesToPlaceLand =  !_allowedActions.ContainsKey(TurnMoveType.PlaceLand) ? 
                Array.Empty<PlaceLandAiEvaluation>() : 
                _allowedActions[TurnMoveType.PlaceLand].
                OrderBy(step => stepsShuffler.Next()).
                OrderByDescending(action => action.Priority).
                Select(action => action.Object).
                Select(action => action as PlaceLandAiEvaluation).ToArray();

            foreach (var action in movesToPlaceLand)
            {
                foreach (LandType land in (LandType[]) Enum.GetValues(typeof(LandType)))
                {
                    _board.SelectLand(Id, land);
                    _board.SelectCell(action.CellToPlace);   
                }
            }
            yield return null;

            
            // --- 4: Analyze the best movement for the existing creatures => perform movement
            // Evaluate
            EvaluateCreatureMovement();
            
            // Perform actions
            var creaturesMovements =  !_allowedActions.ContainsKey(TurnMoveType.MoveCreature) ? 
                Array.Empty<MoveCreatureAiEvaluation>() : 
                _allowedActions[TurnMoveType.MoveCreature].
                OrderBy(step => stepsShuffler.Next()).
                OrderByDescending(action => action.Priority).
                Select(action => action.Object).
                Select(action => action as MoveCreatureAiEvaluation).ToArray();


            foreach (var action in creaturesMovements)
            {
                if (action.CreaturePosition == action.CellToMove) continue;
                _board.SelectCell(action.CreaturePosition);
                _board.SelectCell(action.CellToMove);
            }
            
            
            // Clear data
            ClearActions();
            _board.EndTurn(Id);
            _board = null;
        }


        public override void SetupMatchForTurn(IBoard board)
        {
            board.SetUI(false);
            
            InitializeData(board);
        }

        public override void StartTurn()
        {
            _board.Mono.StartCoroutine(MakeTurn());
        }
    }
}