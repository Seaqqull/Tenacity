using UnityEngine;


namespace Tenacity.General.Items.Consumables
{
    [CreateAssetMenu(fileName = "Coin", menuName = "Items/Coin")]
    public class CoinSO : BaseEnvironmentItemSO<CoinSO, Coin>
    {
        [field: Space]
        [field: SerializeField] public int Count { get; private set; }

    }
}