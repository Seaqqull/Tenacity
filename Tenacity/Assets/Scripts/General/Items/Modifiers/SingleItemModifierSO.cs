using Tenacity.General.Items.Modifiers;
using System.Linq;
using UnityEngine;


namespace Tenacity.General.Items
{
    [CreateAssetMenu(fileName = "SingleModifier", menuName = "Items/Modifiers/Single")]
    public class SingleItemModifierSO : ItemModifierSO
    {
        [SerializeField] protected ItemTypeModificator[] _itemTypeModifiers;
        
        
        public override float GetTradeValue(ItemType type)
        {
            var priceMultiplier = _itemTypeModifiers.Where(modificator => modificator.Type == type)
                .Aggregate(1.0f, (f, modificator) => f * modificator.Modificator);
            return ((priceMultiplier == 0.0f) ? 1.0f : priceMultiplier);
        }
    }
}
