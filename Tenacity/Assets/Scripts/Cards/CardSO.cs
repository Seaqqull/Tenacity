using static Tenacity.Battles.Lands.BattleConstants;
using Tenacity.Battles.Lands.Data;
using Tenacity.Cards.Data;
using System.Reflection;
using Tenacity.General.Items;
using UnityEngine;


namespace Tenacity.Cards
{
    [CreateAssetMenu(fileName = "Card Template", menuName = "Items/Card")]
    public class CardSO : BaseEnvironmentItemSO<CardSO, CardItem>, IInventoryItem
    {
        [SerializeField] private int cardId;
        [SerializeField] private string cardName;
        [SerializeField] private int castingCost;
        [SerializeField] private int life;
        [SerializeField] private int power;
        [SerializeField] private CardType type;
        [SerializeField] private ItemRarity rarity;
        [SerializeField] private string cardText;
        [SerializeField] private LandType landType;
        [SerializeField] private int landCost;
        [SerializeField] private int range;
        [SerializeField] private int movement;
        [SerializeField] private int evaluation;
        [field: Space]
        [field: SerializeField] public Sprite InventoryView { get; private set; }

        
        public override ItemType ItemType => ItemType.Card;
        public override ItemRarity ItemRarity => rarity;
        public override string Name => cardName;
        public override int Id => cardId;

        public int Life => life;
        public int Range => range;
        public int Power => power;
        public CardType Type => type;
        public int LandCost => landCost;
        public int Movement => movement;
        public LandType Land => landType;
        public string CardText => cardText;
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