namespace Tenacity.Cards
{
    public interface IAttackCard
    {
        int Attack { get; set; }
        CardPowerType TypeOfAttack { get; set; }
    }
}