using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDefenseCard
{
    int Defense { get; set; }
    CardPowerType TypeOfDefense { get; set; }
}
