using static Tenacity.Battles.Lands.BattleConstants;
using Tenacity.Battles.Lands.Data;
using Tenacity.General.Items;
using System.Reflection;
using UnityEngine;


namespace Tenacity.Cards
{
    [CreateAssetMenu(fileName = "Card Template", menuName = "Items/Card")]
    public class CardSO : BaseEnvironmentItemSO<CardSO, CardItem>, IInventoryItem
    {
        [SerializeField] private int castingCost;
        [SerializeField] private int life;
        [SerializeField] private int power;
        [SerializeField] private string cardText;
        [SerializeField] private LandType landType;
        [SerializeField] private int landCost;
        [SerializeField] private int range;
        [field: Space]
        [field: SerializeField] public Sprite InventoryView { get; private set; }

        
        public override ItemType ItemType => ItemType.Card;

        public int Life => life;
        public int Range => range;
        public int Power => power;
        public int LandCost => landCost;
        // public int Movement => movement;
        public LandType Land => landType;
        public string CardText => cardText;
        public int CastingCost => castingCost;
        public string CardSprite => InventoryView.name;

        public int Rating => ((Life * GameStateWeights.MinionHealthWeight)
                                  + (Power * GameStateWeights.MinionAttackWeight)
                                  + (Range * GameStateWeights.MinionRangeWeight));
                        // + (Movement * GameStateWeights.MinionMovementWeight));
        public PropertyInfo[] CardProperties => GetType().GetProperties();
    }
}