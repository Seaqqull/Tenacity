using System.Linq;
using UnityEngine;


namespace Tenacity.General.Items.Modifiers
{
    [CreateAssetMenu(fileName = "CompoundModifier", menuName = "Items/Modifiers/Compound")]
    public class CompoundItemModifierSO : SingleItemModifierSO
    {
        [Space]
        [SerializeField] private ItemModifierSO _baseItemModificator;


        public override float GetTradeValue(ItemType type)
        {
            var basePriceModificator = _itemTypeModifiers.Where(modificator => modificator.Type == type)
                .Aggregate(1.0f, (f, modificator) => f * modificator.Modificator);
            return ((basePriceModificator == 0.0f) ? 1.0f : basePriceModificator) * 
                         (_baseItemModificator == null ? 1.0f : _baseItemModificator.GetTradeValue(type));
        }
    }
}
