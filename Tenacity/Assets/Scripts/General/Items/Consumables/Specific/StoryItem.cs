


namespace Tenacity.General.Items.Consumables
{
    public class StoryItem : EnvironmentItem<StoryItemSO, StoryItem>, IConsumable
    {
        public ConsumableTrigger Trigger => ConsumableTrigger.Pickup;
        public int ReusableCount => int.MaxValue;

        
        public void Consume(ConsumableTrigger trigger)
        {
            Destroy(gameObject);
        }
    }
}