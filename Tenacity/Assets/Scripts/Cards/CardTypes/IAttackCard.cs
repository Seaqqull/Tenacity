using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackCard
{
    int Attack { get; set; }
    CardPowerType TypeOfAttack { get; set;}
}
