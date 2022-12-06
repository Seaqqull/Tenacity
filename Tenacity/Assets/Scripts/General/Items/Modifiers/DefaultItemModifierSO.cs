using UnityEngine;


namespace Tenacity.General.Items.Modifiers
{
    [CreateAssetMenu(fileName = "DefaultModifier", menuName = "Items/Modifiers/Default")]
    public class DefaultItemModifierSO : ItemModifierSO
    {
        public override float GetTradeValue(ItemType type)
        {
            return 1.0f;
        }
    }
}
