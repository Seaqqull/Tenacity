using Tenacity.General.SaveLoad.Data;
using System.Collections.Generic;
using Tenacity.Properties;
using Tenacity.Base;
using UnityEngine;
using System.Linq;


namespace Tenacity.General.SaveLoad.Implementation
{
    public class Location : SingleBehaviour<Location>, ISavable
    {
        [SerializeField] private IntegerReference _id;
        
        private List<LocationItem> _locationItems = new ();
        private List<SaveSnap> _itemsSnapshot = new ();
        private LocationSnap _locationSnap;

        public string Id => _id.Value.ToString();


        private void Start()
        {
            if (World.Instance != null)
                World.Instance.AddLocation(this);
        }


        public void AddItem(LocationItem newItem)
        {
            if (!_locationItems.Contains(newItem))
                _locationItems.Add(newItem);
            
            _itemsSnapshot = _itemsSnapshot.Where(item => !item.Id.Equals(newItem.Id)).ToList();
            _itemsSnapshot.Add(newItem.MakeSnap());
            
            // Check whether there exists some item snapshot
            if (_locationSnap?.Items.SingleOrDefault(item => item.Id.Equals(newItem.Id)) is LocationItemSnap itemSnapshot)
            {
                _locationSnap = _locationSnap.UpdateSnap((item => !item.Id.Equals(newItem.Id)));
                newItem.FromSnap(itemSnapshot);
            }
            
            World.Instance.UpdateLocation(this);
        }

        public void RemoveItem(LocationItem newItem)
        {
            if (_locationItems.Contains(newItem))
                _locationItems.Remove(newItem);
        }

        public SaveSnap MakeSnap()
        {
            return new LocationSnap(Id, _itemsSnapshot);
        }

        public void FromSnap(SaveSnap data)
        {
            _locationSnap = data as LocationSnap;
            if ((_locationSnap == null) || !_locationSnap.Id.Equals(Id))
            {
                _locationSnap = null;
                return;
            }

            foreach (var locationItem in _locationItems)
            {
                if (locationItem == null) continue;

                if (_locationSnap?.Items.SingleOrDefault(item => item.Id.Equals(locationItem.Id)) is LocationItemSnap itemSnapshot)
                {
                    _locationSnap = _locationSnap.UpdateSnap((item => !item.Id.Equals(locationItem.Id)));
                    locationItem.FromSnap(itemSnapshot);
                }
            }
            
            World.Instance.UpdateLocation(this);
        }
    }
}