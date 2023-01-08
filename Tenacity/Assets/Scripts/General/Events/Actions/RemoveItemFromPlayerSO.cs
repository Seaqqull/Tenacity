using Tenacity.Properties;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/RemoveItem", fileName = "RemoveItem")]
    public class RemoveItemFromPlayerSO : ActionSO<bool>
    {
        [SerializeField] private IntegerReference _itemId;
        
        
        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return true;
        }

        public void RemoveItem()
        {
            var player = FindObjectOfType<Player.Player>();
            if (player != null)
                player.RemoveItem((item) => item.Id == _itemId.Value);
        }
    }
}