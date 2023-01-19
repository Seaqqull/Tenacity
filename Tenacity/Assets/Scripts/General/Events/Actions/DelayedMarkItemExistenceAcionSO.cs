using Tenacity.General.SaveLoad.Implementation;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/DelayedMarkItemExistence", fileName = "DelayedMarkItemExistence", order = 0)]
    public class DelayedMarkItemExistenceAcionSO : ActionSO<bool>
    {
        private string _itemId;
        
        
        protected override bool PerformAction()
        {
            return true;
        }

        
        public override bool Perform()
        {
            return true;
        }


        public void BackupLocationItem(LocationItem item)
        {
            _itemId = item.Id;
        }

        public void MarkItemAsExistence(bool exist)
        {
            if (_itemId == null) return;
            
            Location.Instance.UpdateItemExistence(new LocationItemSnap(_itemId, exist));
            _itemId = null;
        }
    }
}