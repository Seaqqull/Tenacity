using UnityEngine.Localization;
using UnityEngine;


namespace Tenacity.General.Items.Consumables
{
    [System.Serializable]
    public struct BookPage
    {
        public Sprite Picture;
        public LocalizedString Text;
    }
    
    [CreateAssetMenu(fileName = "Book", menuName = "Items/Book")]
    public class StoryItemSO : BaseEnvironmentItemSO<StoryItemSO, StoryItem>, IInventoryItem
    {
        [field: Space] 
        [field: SerializeField] public BookPage[] Pages { get; private set; }
        [field: SerializeField] public Sprite InventoryView { get; private set; }
    }
}