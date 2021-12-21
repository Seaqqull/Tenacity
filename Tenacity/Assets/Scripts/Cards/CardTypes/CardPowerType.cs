using System;

[Flags]
public enum CardPowerType
{
    None,
    Magician,
    Phisical,
    Combined = Magician | Phisical
}
