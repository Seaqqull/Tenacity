


namespace Tenacity.General.Items
{
    public abstract class EmptyEnvironmentItemSO<T, V> : EnvironmentItemSO<T, V>
        where T : BaseEnvironmentItemSO<T, V>
        where V : EnvironmentItem<T, V>
    {
        
    }
}