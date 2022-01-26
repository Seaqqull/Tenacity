using System;
using UnityEngine;

namespace Tenacity.Cards
{
    [Serializable]
    public class CombinedCard : CardTemplate, IDefenseCard, IAttackCard
    {
        [SerializeField]
        private int attack;
        [SerializeField]
        private CardPowerType attackType;
        [SerializeField]
        private int defense;
        [SerializeField]
        private CardPowerType defenseType;

        public int Attack
        {
            get => attack;
            set => attack = value;
        }
        public CardPowerType TypeOfAttack
        {
            get => attackType;
            set => attackType = value;
        }
        public int Defense
        {
            get => defense;
            set => defense = value;
        }
        public CardPowerType TypeOfDefense
        {
            get => defenseType;
            set => defenseType = value;
        }
    }
}