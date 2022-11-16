using UnityEngine;


namespace Tenacity.General.Items
{
    public class BaseItemSO : ItemSO<BaseItemSO>
    {
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