namespace Tenacity.Cards
{
    public interface IDefenseCard
    {
        int Defense { get; set; }
        CardPowerType TypeOfDefense { get; set; }
    }
}