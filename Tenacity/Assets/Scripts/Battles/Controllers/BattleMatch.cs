using Tenacity.Battles.Generators.Battles.Generators.Interactions;
using Tenacity.Battles.Generators.Battles.Generators.Field;
using Tenacity.Battles.Generators.Creatures;
using Tenacity.Battles.Controllers.Rules;
using Tenacity.Battles.Views.Players;
using Random = UnityEngine.Random;
using Tenacity.Battles.Data.Field;
using Tenacity.Battles.Lands.Data;
using Tenacity.UI.Menus.UI.Menus;
using System.Collections.Generic;
using Tenacity.Battles.Players;
using Tenacity.Battles.Data;
using Tenacity.Managers;
using Tenacity.Cards;
using Tenacity.Base;
using System.Linq;
using UnityEngine;
using System;

// TODO: Vector2.Distance +
// TODO: Team.HasFlag => == +
// TODO: Show proper ground image inside the card view +
// TODO: Update description for the cards (Jump action) +
// TODO: Check whether all cells are only by one player => win +
// TODO: Check whether killing only works on opponent's team creatures & same for movement +
// TODO: Exclude lands, if no cards of such type 
// TODO: Show creatures of different team + 
// TODO: Show end game window with action onClose +
// TODO: Randomize steps +

// TODO: Create action with dialogues to launch the battle +
// TODO: Create reward after winning the game +
// TODO: Handle battle removing from the game world, after you won it +

// TODO: Add Compass to open last location 
// TODO: Compass can be bought from NPC 
// TODO: Add water card to inventory 
namespace Tenacity.Battles.Controllers
{
    public class BattleMatch : SingleBehaviour<BattleMatch>, IBoard 
    {
        #region Constants
        private const int PLAYER_INDEX = 0;
        private const int NO_SELECTION = -1;
        #endregion
        
        [SerializeField] private Transform _fieldCenter;
        [SerializeField] private GameObject _manualUI;
        [Space]
        [SerializeField] private TurnRuleSO _rule;
        [SerializeField] private int _manaPerTurn;
        [Header("Interaction")] 
        [SerializeField] private RaycastManager.RaycastInfo _cellClickPatameter;
        [Header("Field")] 
        [SerializeField] private Vector2 _fieldCellsDistance;
        [Header("Generation")]
        [SerializeField] private CellController _cellPrefab;
        [Space]
        [SerializeField] private InteractionFabricSO _interactionFabric;
        [SerializeField] private CreatureFabricSO _creatureFabric;
        [SerializeField] private CellFabricSO _cellFabric;
        [Space]
        [SerializeField] private PlayerView[] _views;
        
        private int NextPlayerIndex => ((_activePlayerIndex + 1) >= _players.Count) ? 0 : (_activePlayerIndex + 1);
        private IReadOnlyList<Player> _players;
        private CellController[][] _field;
        private int _activePlayerIndex;

        private LandType _selectedLand = LandType.None;
        private CellController _selectedCreatureCell;
        private int _selectedHandCardIndex;

        private bool CardSelected => _selectedHandCardIndex != NO_SELECTION; 
        private bool LandSelected => _selectedLand != LandType.None; 
        
        public Player ActivePlayer { get => _players[_activePlayerIndex]; }
        public NewBattleState State { get; private set; }

        public IReadOnlyList<IPlayer> Players { get => _players; }
        public MonoBehaviour Mono { get => this; }

        public IEnumerable<ICell> AllCells
        {
            get { return _field.SelectMany(cellRow => cellRow.Select(cell => cell)); }
        }
        public ICell[][] Field { get => _field; }

        public Action<bool> OnFinishGame { get; set; }

        

        private void Start()
        {
            StartGame();
            InputManager.MouseLeftButtonAction += OnMouseClick;
        }

        private void OnDestroy()
        {
            InputManager.MouseLeftButtonAction -= OnMouseClick;
        }
        
        
        private void OnMouseClick(bool leftMouseButtonClicked)
        {
            if (leftMouseButtonClicked)
                return;

            var interactionHitInfo = RaycastManager.Instance.GetPoint(_cellClickPatameter);
            if (interactionHitInfo.HitSomePosition)
            {
                var interactionObject = interactionHitInfo.HitData.Object.GetComponent<CellController>();
                if (interactionObject != null)
                    OnFieldCellClick(interactionObject);
            }
        }
        

        public void EndTurn()
        {
            EndTurn(ActivePlayer.Id);
        }

        public void EndTurn(int playerId)
        {
            if ((playerId != ActivePlayer.Id) || (State == NewBattleState.End)) return;
            
            // Clear some code
            if (_players[NextPlayerIndex].IsDead)
            {
                State = NewBattleState.End;
                
                EndBattleMenu.Show();
                EndBattleMenu.Instance.ShowBattleResult(new EndBattleInfo()
                {
                    IsWin = !_players[PLAYER_INDEX].IsDead,
                    InfoText = $"{ActivePlayer.Name} won",
                    OnClose = () => OnFinishGame(!_players[PLAYER_INDEX].IsDead)
                });
                
                return;
            }
            
            StartTurn();
        }

        
        private void StartGame()
        {
            State = NewBattleState.Initialization;
            
            
            var battleData = BattleManager.Instance.GetData();
            BattleManager.Instance.ClearBattleData();
            _players = battleData.Players.Select(player => player as Player).ToList();
            OnFinishGame = battleData.OnFinishGame;
            var playerIndex = -1;
            var rowIndex = -1;
            
            _field = battleData.Field.Select(fieldRow =>
            {
                rowIndex++;
                
                var cellShift = new Vector2(
                    rowIndex * (_fieldCellsDistance.x * 0.5f), 
                    rowIndex * _fieldCellsDistance.y);
                var columnExistingIndex = -1;
                var columnIndex = -1;
                

                return fieldRow.Select<FieldCreationType, CellController>(rowCell =>
                {
                    columnIndex++;
                    if (rowCell == FieldCreationType.Empty)
                        return null;
                    
                    var cell = Instantiate(_cellPrefab, _fieldCenter, false);
                    var localPosition = cell.Transform.localPosition;
                    
                    cell.Transform.localPosition = new Vector3(localPosition.x - cellShift.x + (_fieldCellsDistance.x * columnIndex), localPosition.y, localPosition.z + cellShift.y);
                    cell.FieldPosition = new Vector2Int(rowIndex, ++columnExistingIndex);
                    
                    if (rowCell == FieldCreationType.Ground)
                    {
                        cell.UpdateController(
                            new EmptyCell(),
                            _cellFabric.CreateCell(LandType.None),
                            _interactionFabric.CreateInteraction(),
                            onClick: OnFieldCellClick);
                    }
                    else if (rowCell == FieldCreationType.Player)
                    {
                        var player = _players[++playerIndex];
                        var playerCreature = _creatureFabric.CreatePlayerCreature(player.Team);
                        playerCreature.UpdateCreature(new CreatureData()
                        {
                            Life = player.Health,
                            OnHealthUpdate = (newHealth) =>
                            {
                                player.PerformDamage(player.Health - newHealth);
                                ResetView();
                            },
                            // OnDeath = EndTurn,
                            Team = player.Team,
                            Type = LandType.Neutral
                        });
                        
                        player.View = playerCreature;

                        cell.UpdateController(
                            new PlayerCell(player.Team),
                            _cellFabric.CreateCell(LandType.Neutral),
                            _interactionFabric.CreateInteraction(),
                            creature: playerCreature,
                            onClick: OnFieldCellClick);
                    }
                    else
                        Debug.LogError("[BattleMatch] Error: Wrong field generation type.");
                    return cell;

                }).Where(cell => (cell != null)).ToArray();
            }).ToArray();

            
            // Views
            if (_views.Length != _players.Count)
            {
                Debug.LogError("[BattleMatch] Error: count of views != count of players.");
                return;
            }

            for (int i = 0; i < _views.Length; i++)
            {
                var player = _players[i];
                player.UpdateHand(new PlayerCards(GetRandomCards(player.Hand?.Cards ?? Array.Empty<CardSO>(), player.Deck.Cards, player.Seed, player.HandSize)));

                _views[i].UpdateData(new PlayerDataView()
                {
                    PlayerId = player.Id,

                    MaxHealth = player.MaxHealth,
                    Health = player.Health,
                    GroundEnabled = false,
                    Mana = player.Mana,
                    Name = player.Name,
                    MaxMana = -1,
                    Hand = player.Hand,

                    SelectCard = SelectCard,
                    SelectLand = SelectLand
                });
            }

            // Player turn
            _activePlayerIndex = Random.Range(0, _players.Count);
            StartTurn();
        }
        
        private void StartTurn()
        {
            ResetSelectParameters(true, true, true);
            MarkCellsAsNonSelectable();
            UpdateGroundForPlayers();
            
            _rule.ResetRestrictions();

            ActivePlayer.GainMana(_manaPerTurn);
            ActivePlayer.SetupMatchForTurn(this);

            State = (ActivePlayer.Team == TeamType.Bottom)
                ? NewBattleState.BottomTurn
                : NewBattleState.TopTurn;

            ActivePlayer.UpdateHand(new PlayerCards(GetRandomCards(ActivePlayer.Hand?.Cards ?? Array.Empty<CardSO>(),
                ActivePlayer.Deck.Cards, ActivePlayer.Seed, ActivePlayer.HandSize)));
            
            ResetView();
            ActivePlayer.StartTurn();
        }

        private void ResetView()
        {
            UpdateView(viewData =>
            {
                var player = _players.Single(player => player.Id == viewData.PlayerId);
                return new PlayerDataView(viewData)
                {
                    Health = player.Health,
                    Mana = player.Mana,
                    Hand = player.Hand
                };
            });
        }

        private void CheckForGameEnd()
        {
            if (_players[NextPlayerIndex].IsDead)
                EndTurn();
        }
        
        private void UpdateGroundForPlayers()
        {
            var oldView = _views[_activePlayerIndex];
            _views[_activePlayerIndex].UpdateData(new PlayerDataView(oldView.Data)
            {
                GroundEnabled = false
            });
            
            _activePlayerIndex++;
            if (_activePlayerIndex >= _players.Count)
                _activePlayerIndex = 0;
            
            var newView = _views[_activePlayerIndex];
            _views[_activePlayerIndex].UpdateData(new PlayerDataView(newView.Data)
            {
                GroundEnabled = true
            });
        }
        
        private void MarkCellsAsNonSelectable()
        {
            MarkSelectedCells((row, column, cell) => true, InteractionState.None);
        }
        
        private void OnFieldCellClick(CellController selectedCell)
        {
            if (State == NewBattleState.End) return;
            var activePlayer = ActivePlayer;
            
            
            // --- If nothing was selected before (Because we can't select opponent's cell, we assume its our)
            if (_selectedCreatureCell == null)
            {
                // If select creature ]- We can't select non-creature cell
                if ((selectedCell.State.Type == CellType.Creature) && 
                    activePlayer.Team.HasFlag(selectedCell.CreatureData.Team) && 
                    _rule.IsMoveAvailable(TurnMoveType.MoveCreature, new MoveCreatureContext(selectedCell.Creature)))
                {
                    _selectedCreatureCell = selectedCell;
                    
                    MarkSelectedCells((row, column, fieldCell) =>
                            (((fieldCell.State.Type == CellType.Ground) && CompareLandTypes(fieldCell.State.LandType, selectedCell.Creature.Type))
                                || (((fieldCell.State.Type == CellType.Creature) || (fieldCell.State.Type == CellType.Player)) && !activePlayer.Team.HasFlag(fieldCell.CreatureData.Team))) 
                            && IsCellNear(new Vector2Int(row, column), selectedCell, selectedCell.Creature.Data.Range), 
                        InteractionState.Active
                    );

                    ResetSelectParameters(card: true, land: true);
                    ResetView();
                }
                // If select empty land & some card was selected
                else if (CardSelected &&
                         (selectedCell.State.Type == CellType.Ground) && 
                         selectedCell.State.Team.HasFlag(activePlayer.Team))
                {
                    var selectedCard = activePlayer.Hand.Cards.ElementAt(_selectedHandCardIndex);
                    if ((activePlayer.Mana >= selectedCard.CastingCost) &&
                        CompareLandTypes(selectedCell.State.LandType, selectedCard.Land) &&
                        _rule.IsMoveAvailable(TurnMoveType.PlaceCreature, new PlaceCreatureContext(selectedCard)))
                    {
                        var newCardsList = activePlayer.Hand.Cards.ToList();
                        newCardsList.RemoveAt(_selectedHandCardIndex);
                        
                        activePlayer.UpdateHand(new PlayerCards(newCardsList));
                        activePlayer.SpendMana(selectedCard.CastingCost);
                        
                        var creatureView = _creatureFabric.CreateCardCreature(activePlayer.Team, selectedCard.Land);
                        creatureView.UpdateCreature(new CreatureData()
                        {
                            Life = selectedCard.Life,
                            Power = selectedCard.Power,
                            Team = activePlayer.Team,
                            Type = selectedCard.Land,
                            Range = selectedCard.Range,
                            Priority = selectedCard.Rating
                        });
                        
                        selectedCell.UpdateController(
                            new CreatureCell(new LandCell(selectedCell.State.Team, selectedCell.State.LandType)), 
                            creature:creatureView
                        );

                        
                        _rule.DoMove(TurnMoveType.PlaceCreature, new PlaceCreatureContext(selectedCard));
                        _rule.DoMove(TurnMoveType.MoveCreature, new MoveCreatureContext(creatureView));
                        
                        MarkCellsAsNonSelectable();
                        UpdateView(viewData => new PlayerDataView(viewData)
                        {
                            Hand = (viewData.PlayerId == activePlayer.Id) ? activePlayer.Hand : viewData.Hand,
                            Mana = (viewData.PlayerId == activePlayer.Id) ? activePlayer.Mana : viewData.Mana
                        });
                        
                        ResetSelectParameters(card: true);
                    }
                }
                // If Select empty cell & placement was selected
                else if (LandSelected && 
                         (selectedCell.State.Type == CellType.None) &&
                         (selectedCell.SelectionState == InteractionState.Active) &&
                         _rule.IsMoveAvailable(TurnMoveType.PlaceLand, new PlaceLandContext(_selectedLand)))
                {
                    selectedCell.UpdateController(new LandCell(activePlayer.Team, _selectedLand), _cellFabric.CreateCell(_selectedLand));
                    MarkCellsAsNonSelectable();
                    ResetView();
                    
                    _rule.DoMove(TurnMoveType.PlaceLand, new PlaceLandContext(_selectedLand));
                    
                    ResetSelectParameters(land: true);
                }
                return;
            }
            
            // --- If something already was selected ]- creature before
            // The same cell
            if (_selectedCreatureCell == selectedCell)
            {
                ResetSelectParameters(creature: true);
                MarkCellsAsNonSelectable();
            }
            // Clicked on Ground
            else if ((selectedCell.State.Type == CellType.Ground) &&
                     (selectedCell.SelectionState == InteractionState.Active) &&
                     CompareLandTypes(selectedCell.State.LandType, _selectedCreatureCell.Creature.Type))
            {
                _rule.DoMove(TurnMoveType.MoveCreature, new MoveCreatureContext(_selectedCreatureCell.Creature));
                var creature = _selectedCreatureCell.Creature;
                
                _selectedCreatureCell.UpdateController(new LandCell(_selectedCreatureCell.State.Team, _selectedCreatureCell.State.LandType));
                _selectedCreatureCell.ResetController(false, true, false, false, false);
                MarkCellsAsNonSelectable();

                selectedCell.UpdateController(new CreatureCell(selectedCell.State as LandCell), creature: creature);
                ResetSelectParameters(creature: true);
            }
            // Clicked on Player | Creature 
            else if (((selectedCell.State.Type == CellType.Creature) && selectedCell.CreatureData.Team.HasFlag(activePlayer.Team)) || 
                ((selectedCell.SelectionState == InteractionState.Active) && 
                     ((selectedCell.State.Type == CellType.Player) || (selectedCell.State.Type == CellType.Creature))))
            {
                if (!selectedCell.CreatureData.Team.HasFlag(activePlayer.Team))
                {
                    _rule.DoMove(TurnMoveType.MoveCreature, new MoveCreatureContext(_selectedCreatureCell.Creature));
                    var playerCreature = _selectedCreatureCell.Creature;
                    var opponentCreature = selectedCell.Creature;

                    // Damage the opponent
                    opponentCreature.PerformDamage(playerCreature.Data.Power);
                    if (opponentCreature.Health <= 0)
                    {
                        selectedCell.ResetController(false, true, false, false);
                        selectedCell.UpdateController(new LandCell(selectedCell.State.Team, selectedCell.State.LandType));
                    }
                    
                    // Damage as the response
                    playerCreature.PerformDamage(opponentCreature.Data.Power);
                    if (playerCreature.Health <= 0)
                    {
                        _selectedCreatureCell.ResetController(false, true, false, false);
                        _selectedCreatureCell.UpdateController(new LandCell(_selectedCreatureCell.State.Team, _selectedCreatureCell.State.LandType));
                    }
                    
                    
                    ResetSelectParameters(creature: true);
                    MarkCellsAsNonSelectable();
                    
                    CheckForGameEnd();
                }
                else if (selectedCell.State.Type == CellType.Creature)
                {
                    ResetSelectParameters(creature: true);
                    MarkCellsAsNonSelectable();

                    OnFieldCellClick(selectedCell);
                }
            }
        }
        
        private bool CompareLandTypes(LandType from, LandType with)
        {
            return (with == LandType.Neutral) || from.HasFlag(with);
        }

        private void UpdateView(Func<PlayerDataView, PlayerDataView> viewUpdater)
        {
            for (int i = 0; i < _views.Length; i++)
            {
                var updatedData = viewUpdater(_views[i].Data);
                _views[i].UpdateData(updatedData);
            }
        }

        private bool IsCellNear(Vector2Int fieldPosition, CellController cell, int distance)
        {
            return IsCellNear(cell.FieldPosition, fieldPosition, distance);
        }

        private bool IsCellNear(Vector2Int cellPosition, Vector2Int compareCellPosition, int distance)
        {
            var minMaxRowIndex = new Vector2Int(
                ((cellPosition.x - distance) < 0) ? 0 : (cellPosition.x - distance),
                ((cellPosition.x + distance) >= _field.Length) ? _field.Length - 1 : cellPosition.x + distance);
            if ((compareCellPosition.x < minMaxRowIndex.x) || (compareCellPosition.x > minMaxRowIndex.y)) return false;

            var i = compareCellPosition.x;
            distance -= (cellPosition.x == compareCellPosition.x) ? 0 :
                (Math.Abs(cellPosition.x - compareCellPosition.x) - 1);
            
            
            Vector2Int minMaxColumnIndex;
            if (cellPosition.x < (_field.Length / 2))
            {
                minMaxColumnIndex = new Vector2Int(
                    (i <= cellPosition.x) ? Math.Clamp((cellPosition.y - distance), 0, _field[i].Length - 1) : Math.Clamp((cellPosition.y - distance + 1), 0, _field[i].Length - 1),
                    (i < cellPosition.x) ? Math.Clamp((cellPosition.y + distance - 1), 0, _field[i].Length - 1) : Math.Clamp((cellPosition.y + distance), 0, _field[i].Length - 1)
                );
            }
            else if (cellPosition.x == (_field.Length / 2))
            {
                minMaxColumnIndex = new Vector2Int(
                    Math.Clamp((cellPosition.y - distance), 0, _field[i].Length - 1),
                    (i != cellPosition.x) ? Math.Clamp((cellPosition.y + distance - 1), 0, _field[i].Length - 1) : Math.Clamp((cellPosition.y + distance), 0, _field[i].Length - 1)
                );
            }
            else
            {
                minMaxColumnIndex = new Vector2Int(
                    (i >= cellPosition.x) ? Math.Clamp((cellPosition.y - distance), 0, _field[i].Length - 1) : Math.Clamp((cellPosition.y - distance + 1), 0, _field[i].Length - 1),
                    (i > cellPosition.x) ? Math.Clamp((cellPosition.y + distance - 1), 0, _field[i].Length - 1) : Math.Clamp((cellPosition.y + distance), 0, _field[i].Length - 1)
                );
            }
            return ((compareCellPosition.y >= minMaxColumnIndex.x) && (compareCellPosition.y <= minMaxColumnIndex.y));
        }
        
        private bool IsCellNear(int row, int column, int distance, TeamType team, Func<CellController, bool> selector = null)
        {
            var minMaxRowIndex = new Vector2Int(
                ((row - distance) < 0) ? 0 : (row - distance),
                ((row + distance) >= _field.Length) ? _field.Length - 1 : row + distance);
            for (int i = minMaxRowIndex.x; i <= minMaxRowIndex.y; i++)
            {
                Vector2Int minMaxColumnIndex;
                if (row < (_field.Length / 2))
                {
                    minMaxColumnIndex = new Vector2Int(
                        (i <= row) ? Math.Clamp((column - distance), 0, _field[i].Length - 1) : Math.Clamp((column - distance + 1), 0, _field[i].Length - 1),
                        (i < row) ? Math.Clamp((column + distance - 1), 0, _field[i].Length - 1) : Math.Clamp((column + distance), 0, _field[i].Length - 1)
                    );
                }
                else if (row == (_field.Length / 2))
                {
                    minMaxColumnIndex = new Vector2Int(
                        Math.Clamp((column - distance), 0, _field[i].Length - 1),
                        (i != row) ? Math.Clamp((column + distance - 1), 0, _field[i].Length - 1) : Math.Clamp((column + distance), 0, _field[i].Length - 1)
                    );
                }
                else
                {
                    minMaxColumnIndex = new Vector2Int(
                        (i >= row) ? Math.Clamp((column - distance), 0, _field[i].Length - 1) : Math.Clamp((column - distance + 1), 0, _field[i].Length - 1),
                        (i > row) ? Math.Clamp((column + distance - 1), 0, _field[i].Length - 1) : Math.Clamp((column + distance), 0, _field[i].Length - 1)
                    );
                }
                for (int j = minMaxColumnIndex.x; j <= minMaxColumnIndex.y; j++)
                    if  ((_field[i][j].State.Team == team) && ((selector == null) || selector(_field[i][j])))
                        return true;
            }
            return false;
        }
        
        private void MarkSelectedCells(Func<int, int, CellController, bool> selector, InteractionState state)
        {
            for (int i = 0; i < _field.Length; i++)
            {
                for (int j = 0; j < _field[i].Length; j++)
                {
                    _field[i][j].SwitchSelection(selector(i, j, _field[i][j]) ? state : InteractionState.None);
                }
            }
        }
        
        public void ResetSelectParameters(bool card = false, bool land = false, bool creature = false)
        {
            if (card)
                _selectedHandCardIndex = NO_SELECTION;
            if (land)
                _selectedLand = LandType.None;
            if (creature)
                _selectedCreatureCell = null;
        }

        private IEnumerable<CardSO> GetRandomCards(IEnumerable<CardSO> storage, IEnumerable<CardSO> source, int seed, int desiredCount)
        {
            var selectedCards = storage.ToList();
            var cardsToAdd = (desiredCount - selectedCards.Count);
            var sourceCardsCount = source.Count();
            var takeIndex = 0;
            source = source.OrderBy(card => seed);

            for (int i = 0; i < cardsToAdd && sourceCardsCount != 0; i++, takeIndex++)
            {
                selectedCards.Add(source.ElementAt(takeIndex % sourceCardsCount));
                if ((takeIndex == (sourceCardsCount - 1)) && (selectedCards.Count < desiredCount))
                {
                    takeIndex = 0;
                    seed *= 2;
                    
                    source = source.OrderBy(card => seed);
                }
            }
            
            return selectedCards.Take(desiredCount).ToList();
        }
        
        
        public void SetUI(bool manualInput)
        {
            _manualUI.SetActive(manualInput);
        }

        public TeamType OpponentTeam(TeamType team)
        {
            return (team == TeamType.Bottom) ? TeamType.Top : TeamType.Bottom;
        }

        
        public bool SelectLand(int playerId, LandType land)
        {
            if ((ActivePlayer.Id != playerId) || (State == NewBattleState.End)) return false;


            var selectLand = (land != LandType.None) && 
                             ((_selectedLand != land) && _rule.IsMoveAvailable(TurnMoveType.PlaceLand, new PlaceLandContext(land)));
            _selectedLand =  selectLand ? land : LandType.None;
            ResetSelectParameters(card: true, creature: true);
            
            MarkSelectedCells((row, column, cell) =>
                (cell.State.LandType == LandType.None) && IsCellNear(row, column, 1, ActivePlayer.Team), 
                selectLand? InteractionState.Active : InteractionState.None
            );
            ResetView();
            
            return selectLand;
        }
        
        public bool SelectCard(int playerId, int handCardIndex)
        {
            if ((ActivePlayer.Id != playerId) || (State == NewBattleState.End)) return false;


            // Also check for -1
            var newlySelectedCard = ActivePlayer.Hand.Cards.ElementAt(handCardIndex);
            var selectCard = (_selectedHandCardIndex != handCardIndex) &&
                             (ActivePlayer.Mana >= newlySelectedCard.CastingCost) &&
                 IsEnoughCards(cell => (cell.State.LandType == newlySelectedCard.Land), ActivePlayer.Team, newlySelectedCard.LandCost) &&
                 _rule.IsMoveAvailable(TurnMoveType.PlaceCreature, new PlaceCreatureContext(newlySelectedCard));
            
            _selectedHandCardIndex =  selectCard ? handCardIndex : NO_SELECTION;
            ResetSelectParameters(land: true, creature: true);
            
            MarkSelectedCells((row, column, cell) => (cell.State.Type == CellType.Ground) && 
                                                     (cell.State.Team == ActivePlayer.Team) &&
                                                     CompareLandTypes(cell.State.LandType, newlySelectedCard.Land), 
                selectCard? InteractionState.Active : InteractionState.None
            );
            ResetView();
            
            return selectCard;
        }

        public void DeselectCard(int playerId)
        {
            SelectCard(playerId, _selectedHandCardIndex);
        }

        public int DistanceBetweenCells(ICell fromCell, ICell toCell)
        {
            return Math.Abs(fromCell.FieldPosition.x - toCell.FieldPosition.x) + Math.Abs(fromCell.FieldPosition.y - toCell.FieldPosition.y);
        }

        public int CountOfCards(ICell cell, Func<ICell, bool> selector, int distance = -1)
        {
            var cellIndex = new Vector2Int(cell.FieldPosition.x, cell.FieldPosition.y);
            var searchDistance = distance == -1 ? int.MaxValue : distance;
            var countOfCards = 0;

            for (int i = 0; i < _field.Length; i++)
            {
                var cellsRow = _field[i];
                for (int j = 0; j < cellsRow.Length; j++)
                {
                    if (selector(cellsRow[j]) && IsCellNear(new Vector2Int(i, j), cellIndex, searchDistance))
                        countOfCards++;
                }
            }
            return countOfCards;
        }

        public void ResetCellSelection()
        {
            ResetSelectParameters(creature: true);
            MarkCellsAsNonSelectable();
        }
        public void SelectCell(Vector2Int fieldPosition)
        {
            OnFieldCellClick(_field[fieldPosition.x][fieldPosition.y]);
        }
        
        public int CountOfCards(Func<ICell, bool> selector, TeamType team)
        {
            return _field.SelectMany(cellsRow => cellsRow.Select(cell => cell))
                .Count(cell => cell.State.Team.HasFlag(team) && selector(cell));
        }
        
        public bool IsEnoughCards(Func<ICell, bool> selector, TeamType team, int count)
        {
            return CountOfCards(selector, team) >= count;
        }
    }
}