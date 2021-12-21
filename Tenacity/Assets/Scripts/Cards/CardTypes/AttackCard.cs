using System;
using UnityEngine;

[Serializable]
public class AttackCard : Card, IAttackCard
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
