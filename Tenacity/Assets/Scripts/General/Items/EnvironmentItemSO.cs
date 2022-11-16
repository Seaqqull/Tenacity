using UnityEngine;


namespace Tenacity.General.Items
{
    public abstract class EnvironmentItemSO<T, V> : ItemSO<T>
    where T : ItemSO<T>
    where V : Item<T>
    {
        [SerializeField] private V _visualization;


        public V Visualization { get => _visualization; }
    }
}