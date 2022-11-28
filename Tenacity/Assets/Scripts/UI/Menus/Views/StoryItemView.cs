using Tenacity.General.Items.Consumables;
using Tenacity.General.Items;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace Tenacity.UI.Menus.Views
{
    public class StoryItemView : ItemView
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _header;
        [Space]
        [SerializeField] private Button _previousPage;
        [SerializeField] private Button _nextPage;
        
        private StoryItemSO _storyItem;
        private int _pageIndex;

        public override ItemType ViewType => ItemType.Story;

        
        private void UpdatePages(int pageIndex)
        {
            _pageIndex = pageIndex;
            _nextPage.gameObject.SetActive(_pageIndex < (_storyItem.Pages.Length - 1));
            _previousPage.gameObject.SetActive(_pageIndex > 0);
            
            var page = _storyItem.Pages[_pageIndex];
            
            _image.sprite = page.Picture;
            _description.text = page.Text.GetLocalizedString();
            _header.text = _storyItem.Name;
        }
        

        public void NextPage()
        {
            if (_pageIndex < (_storyItem.Pages.Length - 1))
                UpdatePages(_pageIndex + 1);
        }

        public void PreviousPage()
        {
            if (_pageIndex > 0)
                UpdatePages(_pageIndex - 1);
        }
        
        public override void ShowItemData(IItem item)
        {
            _storyItem = item as StoryItemSO;
            if (_storyItem == null) return;
            
            UpdatePages(0);
        }
    }
}
