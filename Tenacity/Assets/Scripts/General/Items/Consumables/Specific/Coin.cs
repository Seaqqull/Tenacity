


namespace Tenacity.General.Items.Consumables
{
    public class Coin : EnvironmentItem<CoinSO, Coin>, IConsumable
    {
        public ConsumableTrigger Trigger => ConsumableTrigger.Pickup;
        public int ReusableCount  => 1;
        public int Count => Data.Count;
        
        
        public void Consume(ConsumableTrigger trigger)
        {
            Destroy(gameObject);
        }
    }
}