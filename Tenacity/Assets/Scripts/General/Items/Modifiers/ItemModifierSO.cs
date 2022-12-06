using UnityEngine;


namespace Tenacity.General.Items.Modifiers
{
    [System.Serializable]
    public struct ItemTypeModificator
    {
        public ItemType Type;
        public float Modificator;
    }

    public interface IItemModifier
    {
        public float GetTradeValue(ItemType type);
    }

    public class DefaultItemModifier : IItemModifier
    {
        private static DefaultItemModifier _instance;

        public static DefaultItemModifier Instance
        {
            get => _instance ??= new DefaultItemModifier();
        }

        public float GetTradeValue(ItemType type)
        {
            return 1.0f;
        }
    }
    

    public abstract class ItemModifierSO : ScriptableObject, IItemModifier
    {
        public abstract float GetTradeValue(ItemType type);
    }
}
