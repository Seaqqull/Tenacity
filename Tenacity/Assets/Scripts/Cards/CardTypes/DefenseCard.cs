using System;
using UnityEngine;

namespace Tenacity.Cards
{
    [Serializable]
    public class DefenseCard : CardTemplate, IDefenseCard
    {
        [SerializeField]
        private int defense;
        [SerializeField]
        private CardPowerType defenseType;

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