using Tenacity.Battles.Lands.Data;
using Tenacity.Cards.Inventory;
using Tenacity.Cards.Data;
using System.Reflection;
using UnityEngine;
using static Tenacity.Battles.Lands.BattleConstants;

namespace Tenacity.Cards
{
    [CreateAssetMenu(fileName = "Card Template", menuName = "Card")]
    public class CardDataSO : ScriptableObject, IItem
    {
        [SerializeField] private int cardId;
        [SerializeField] private string cardName;
        [SerializeField] private int castingCost;
        [SerializeField] private int life;
        [SerializeField] private int power;
        [SerializeField] private CardType type;
        [SerializeField] private RarityType rarity;
        [SerializeField] private string cardText;
        [SerializeField] private LandType landType;
        [SerializeField] private int landCost;
        [SerializeField] private int range;
        [SerializeField] private int movement;
        [SerializeField] private int evaluation;


        public ItemType ItemType => ItemType.Card;

        public int Life => life;
        public int Range => range;
        public int Power => power;
        public int CardId => cardId;
        public CardType Type => type;
        public string Name => cardName;
        public int LandCost => landCost;
        public int Movement => movement;
        public LandType Land => landType;
        public string CardText => cardText;
        public RarityType Rarity => rarity;
        public int Evaluation => evaluation;
        public int CastingCost => castingCost;
        public int CardRating => ((Life * GameStateWeights.MinionHealthWeight)
                        + (Power * GameStateWeights.MinionAttackWeight)
                        + (Range * GameStateWeights.MinionRangeWeight)
                        + (Movement * GameStateWeights.MinionMovementWeight));
        public string RaritySprite => $"rarity_{rarity}";
        public string CardLandSprite => $"fraction_{landType}";
        public string CardSprite => type + "_" +cardId.ToString();
        public PropertyInfo[] CardProperties => GetType().GetProperties();

    }
}