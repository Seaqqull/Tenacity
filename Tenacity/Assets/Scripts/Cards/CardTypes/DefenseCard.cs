using System;
using UnityEngine;

[Serializable]
public class DefenseCard : Card, IDefenseCard
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
