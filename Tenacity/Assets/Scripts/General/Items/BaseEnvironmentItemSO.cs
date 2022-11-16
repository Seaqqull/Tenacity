using UnityEngine;


namespace Tenacity.General.Items
{
    public class BaseEnvironmentItemSO<T, V> : EnvironmentItemSO<T, V>
        where T : BaseEnvironmentItemSO<T, V>
        where V : EnvironmentItem<T, V>
    {
        [Space]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private ItemType _type;
        [SerializeField] private ItemRarity _rarity;
        
        public override ItemRarity ItemRarity { get => _rarity; }
        public override ItemType ItemType { get => _type; }
        public override string Name { get => _name; }
        public override int Id { get => _id; }
    }
}