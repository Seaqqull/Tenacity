using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tenacity.Battles.Controllers;
using Tenacity.Battles.Data;
using Tenacity.Battles.Lands;
using Tenacity.Battles.Lands.Data;
using Tenacity.Cards;
using Tenacity.Cards.Data;
using UnityEngine;
using static Tenacity.Battles.Lands.BattleConstants;

namespace Tenacity.Battles.Enemies
{
    [System.Serializable]
    public class BattleEnemyAI
    {
        [SerializeField] private BattleEnemyController Enemy;
        [SerializeField] private int _threshold;

        public BattleEnemyController Player { get; set; }
        public EnemyDecisionMode CurrentDecisionMode { get; private set; }


        private void HandCardsAction()
        {
            DecidePlaceHandCard(out Card minionToPlace, out Land landToPlace);

            if (minionToPlace == null || landToPlace == null)
            {
                DecidePlaceLand();
                return;
            }
            if (landToPlace.Type == LandType.None)
            {
                LandType landType = LandType.Neutral;
                if (minionToPlace.Data.LandCost - Player.LandCounts[minionToPlace.Data.Land] <= 1)
                    landType = minionToPlace.Data.Land;
                Player.PlaceLandOnBoard(landType, landToPlace);
            }
            Player.PlaceHandCardOnBoard(minionToPlace, landToPlace);
        }

        private void MinionAction(Card minion)
        {
            int currentMinionRating = Player.CountMinionRating(minion);

            var cardToAttack = DecideMinionAttackMode(minion, currentMinionRating, out int attackActionRating);
            var selectedLand = DecideMinionMove(minion, currentMinionRating, out int moveActionRating);

            if (cardToAttack != null)
            {
                Player.Attack(minion, cardToAttack);
            }
            else Player.MoveMinion(minion, selectedLand);

        }

        private void DecidePlaceLand()
        {
            var lands = Player.AvailableLandCells.Where(land => land.Type == LandType.None && land != Player.Hero.GetComponentInParent<Land>());

            Land targetLand = null;
            var maxRating = 0f;
            foreach (Land land in lands)
            {
                float rating = 10/FindDistanceToEnemyHero(land) + Player.CountEnemiesInRangeRating(land)
                    + Player.CountMinionsInRangeRating(land);
                if (rating > maxRating)
                {
                    targetLand = land;
                    maxRating = rating;
                }
            }

            var landType = LandType.Neutral;
            var selectedHandCard = Player.Hand.OrderByDescending(card => card.Data.CardRating).FirstOrDefault();
            if (selectedHandCard != null 
                && selectedHandCard.Data.LandCost > Player.LandCounts[selectedHandCard.Data.Land])
                landType = selectedHandCard.Data.Land;
            Player.PlaceLandOnBoard(landType, targetLand);
        }

        private bool DecidePlaceHandCard(out Card minionToPlace, out Land landToPlace)
        {
            int maxPlaceRating = 0;
            minionToPlace = null;
            landToPlace = null;

            foreach (Land land in Player.FreeAvailableLands)
            {
                var placeRating = (int)(Player.CountEnemiesInRangeRating(land) + FindDistanceToEnemyHero(land) / GameStateRatings.MinionDistanceToEnemyPerTile);
                if (placeRating > maxPlaceRating)
                {
                    maxPlaceRating = placeRating;
                    landToPlace = land;
                }
            }

            if (Player.AvailableHandCards.Count == 0) return false;
            if (Player.MinionsRating > 30 && Mathf.Abs(Player.BoardRating - Enemy.BoardRating) < _threshold) return false;

            minionToPlace = Player.AvailableHandCards.FindAll(card => card.Data.LandCost - Player.LandCounts[card.Data.Land] <= 1).OrderByDescending(card => card.Data.CardRating).FirstOrDefault();

            return true;
        }

        /*
         * foreach (Card minion in _player.Minions){
                int minionLandPlacingRating;
                var land = AnalizeWastedLansToMoveMinion(minion, CountMinionRating(_player, minion), out minionLandPlacingRating);
                if (minionLandPlacingRating > maxLandPlacingRating)
                {
                    maxLandPlacingRating = minionLandPlacingRating;
                    selectedCellToPlace = land;
                }
            }
            foreach (Card handCard in _player.Hand)
            {
                int handCardLandPlacingRating = handCard.Data.CardRating;
                for 
                
            }
            return null;
         */
        private Land SelectLandToMoveMinion(Card minion, int currentMinionRating, List<Land> landsToPlace, out int maxActionRating)
        {
            float currentDistance = FindDistanceToEnemyHero(minion.GetComponentInParent<Land>());

            Land selectedLand = null;
            maxActionRating = 0;

            foreach (Land landToMove in landsToPlace)
            {
                var movedCreatureRating = minion.Data.CardRating
                    + Player.CountEnemiesInRangeRating(landToMove)
                    + GetRatingOfDistanceToEnemyHero(landToMove, currentDistance)
                    + GameStateRatings.MinionMovedRating;
                if (movedCreatureRating > maxActionRating)
                {
                    selectedLand = landToMove;
                    maxActionRating = movedCreatureRating;
                }
            }
            if (Mathf.Abs(currentMinionRating - maxActionRating) < _threshold) return null;
            return selectedLand;
        }

        private Land AnalizeWastedLandsToMoveMinion(Card minion, int currentMinionRating, out int maxActionRating)
        {
            List<Land> availableLands = minion.GetComponentInParent<Land>().NeighborLands.FindAll((el) => el.Type == Lands.Data.LandType.None);
            return SelectLandToMoveMinion(minion, currentMinionRating, availableLands, out maxActionRating);
        }

        private Land DecideMinionMove(Card minion, int currentMinionRating, out int maxActionRating)
        {
            maxActionRating = 0;
            if (minion == null) return null;
            List<Land> availableLands = minion.GetComponentInParent<Land>().NeighborLands
                .FindAll((el) => el.IsAvailableForCards && el.Type.HasFlag(minion.Data.Land));
            return SelectLandToMoveMinion(minion, currentMinionRating, availableLands, out maxActionRating);
        }


        private Card DecideMinionAttackMode(Card minion, int currentMinionRating, out int maxActionRating)
        {
            maxActionRating = 0;
            if (minion == null) return null;

            Card selectedCreatureToAttack = null;

            foreach (Card creatureToAttack in Player.GetCreaturesToAttack(minion.GetComponentInParent<Land>()))
            {
                var attackRating = currentMinionRating + Enemy.CountMinionRating(creatureToAttack) + GameStateRatings.MinionAttackRating;
                if (creatureToAttack.CurrentLife - minion.Data.Power <= 0) attackRating += GameStateRatings.MinionKillerRating;
                if (minion.CurrentLife - creatureToAttack.Data.Power <= 0) attackRating -= GameStateRatings.MinionAttackRating;
                if (creatureToAttack.Data.Type == CardType.Hero)
                {
                    maxActionRating = attackRating;
                    return creatureToAttack;
                }
                if (attackRating > maxActionRating)
                {
                    selectedCreatureToAttack = creatureToAttack;
                    maxActionRating = attackRating;
                }
            }
            return selectedCreatureToAttack;
        }


        private float FindDistanceToEnemyHero(Land land)
        {
            if (land == null) return float.NaN;
            return Mathf.Abs(Vector2.Distance(Enemy.Hero.GetComponentInParent<Land>().CellPosition, land.CellPosition));
        }

        private int GetRatingOfDistanceToEnemyHero(Land selectedLand, float currentDistance)
        {
            float distance = Mathf.Abs(Vector2.Distance(Enemy.Hero.GetComponentInParent<Land>().CellPosition, selectedLand.CellPosition));
            return (distance < currentDistance) ? GameStateRatings.MinionDistanceToEnemyPerTile : 0;
        }


        public void ManageAllCardsActions()
        {
            HandCardsAction();
            foreach (Card minion in Player.Minions) MinionAction(minion);
        }

    }
}