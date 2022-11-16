using UnityEngine;


namespace Tenacity.General.Items.Consumables
{
    public class Coin : EnvironmentItem<CoinSO, Coin>, IConsumable
    {
        [SerializeField] private int _count;
        
        public ConsumableTrigger Trigger => ConsumableTrigger.Pickup;
        public int ReusableCount  => 1;
        public int Count => _count;
        
        
        public void Consume(ConsumableTrigger trigger)
        {
            Destroy(gameObject);
        }
    }
}