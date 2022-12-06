using UnityEngine;


namespace Tenacity.General.Items.Modes
{
    [CreateAssetMenu(fileName = "ItemPrice", menuName = "Items/Modes/Price", order = 0)]
    public class PriceItemMode : ItemMode
    {
        [field: SerializeField] public int Price { get; private set; }
    }
}