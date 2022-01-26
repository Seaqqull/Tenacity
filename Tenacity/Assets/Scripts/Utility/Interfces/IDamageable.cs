


namespace Tenacity.Utility.Interdaces
{
    /// <summary>
    /// Signalize that entity can absorb some damage
    /// </summary>
    public interface IDamageable
    {
        public void PerformDamage(int amount);
    }
}