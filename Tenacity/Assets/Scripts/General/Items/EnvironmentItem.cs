


namespace Tenacity.General.Items
{
    public class EnvironmentItem<T, V> : Item<T>
        where T : EnvironmentItemSO<T, V>
        where V : EnvironmentItem<T, V>
    {
        
    }
}