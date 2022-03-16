using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Tenacity.Lands;
using UnityEngine;

namespace Tenacity.Cards
{
    public enum CardType
    {
        Creature = 0,
        Structure = 1,
        Event = 2,
    }
    public enum RarityType
    {
        Common = 0,
        Rare = 1,
        Legendary = 2,
    }

    [CreateAssetMenu(fileName = "Card Template", menuName = "Card")]
    public class CardData : ScriptableObject
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

        public int Life => life;
        public int Power => power;
        public int CardId => cardId;
        public CardType Type => type;
        public string Name => cardName;
        public int LandCost => landCost;
        public LandType Land => landType;
        public string CardText => cardText;
        public RarityType Rarity => rarity;
        public int CastingCost => castingCost;
        public string RaritySprite => $"rarity_{rarity}";
        public string CardLandSprite => $"fraction_{landType}";
        public string CardSprite => type + "_" +cardId.ToString();
        public PropertyInfo[] CardProperties => GetType().GetProperties();
 
    }
}