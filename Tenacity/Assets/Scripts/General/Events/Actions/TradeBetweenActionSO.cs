using Tenacity.General.Items.Modifiers;
using Tenacity.Cards.Inventory;
using UnityEngine.Events;
using Tenacity.UI.Menus;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/TradeBetween", fileName = "TradeBetween", order = 0)]
    public class TradeBetweenActionSO : ActionSO<bool>
    {
        [SerializeField] private EntityInventory _sourceInventory;
        [Space] 
        [SerializeField] private ItemModifierSO _sourcePriceModifier;
        [SerializeField] private ItemModifierSO _targetPriceModifier;
        [Space]
        [SerializeField] private UnityEvent _onPerform;
        [SerializeField] private UnityEvent _onClose;

        
        public override bool Perform()
        {
            return true;
        }
        
        protected override bool PerformAction()
        {
            return true;
        }

        public void Perform(EntityInventory inventoryToTradeWIth)
        {
            _onPerform.Invoke();
            
            TradeMenu.Show();
            TradeMenu.Instance.Initialize(
                _sourceInventory, 
                inventoryToTradeWIth, 
                _sourcePriceModifier, 
                _targetPriceModifier
            );
            TradeMenu.Instance.OnClose += _onClose.Invoke;
        }
    }
}