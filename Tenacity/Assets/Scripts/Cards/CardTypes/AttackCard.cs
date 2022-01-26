using System;
using UnityEngine;

namespace Tenacity.Cards
{
    [Serializable]
    public class AttackCard : CardTemplate, IAttackCard
    {
        [SerializeField]
        private int attack;
        [SerializeField]
        private CardPowerType attackType;

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

    }
}