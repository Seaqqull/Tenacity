using System;


namespace Tenacity.General.Items.Consumables
{
    [Flags]
    public enum ConsumableTrigger { None, Pickup, Use }

    public interface IConsumable
    {
        public ConsumableTrigger Trigger { get; }
        public int ReusableCount { get; }
        
        public void Consume(ConsumableTrigger trigger);
    }
}
