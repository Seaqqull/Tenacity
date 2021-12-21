using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class CombinedCard : Card, IDefenseCard, IAttackCard
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
