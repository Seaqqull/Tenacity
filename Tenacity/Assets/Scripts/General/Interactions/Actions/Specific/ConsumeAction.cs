using Tenacity.General.Items.Consumables;
using Tenacity.General.Items;
using UnityEngine;


namespace Tenacity.General.Interactions.Actions
{
    [CreateAssetMenu(menuName = "Action/Interaction/ConsumeItem", fileName = "ConsumeItem")]
    public class ConsumeAction : InteractionAction
    {
        [SerializeField] private ConsumableTrigger _consumyType;
        
        
        public override void Execute(Interaction interaction, Collider intruder)
        {
            var player = intruder.GetComponent<Player.Player>();
            var item = interaction.GetComponent<IItem>();
            
            player?.Consume(item, _consumyType);
        }
    }
}