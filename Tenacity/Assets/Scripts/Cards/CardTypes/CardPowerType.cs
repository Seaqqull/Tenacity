using System;

namespace Tenacity.Cards
{
    [Flags]
    public enum CardPowerType
    {
        None,
        Magician,
        Phisical,
        Combined = Magician | Phisical
    }
}