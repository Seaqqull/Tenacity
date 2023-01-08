using Tenacity.General.SaveLoad.Data;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


namespace Tenacity.General.SaveLoad.Implementation
{
    [System.Serializable]
    public class LocationItemSnap : SaveSnap
    {
        public bool Exists { get; private set; }

        
        public LocationItemSnap(MonoBehaviour behaviour, bool exists = true) : base(behaviour)
        {
            Exists = exists;
        }
        
        public LocationItemSnap(string id, bool exists = true) : base(id)
        {
            Exists = exists;
        }
    }

    [System.Serializable]
    public class LocationSnap : SaveSnap
    {
        public SaveSnap[] Items { get; private set; } = Array.Empty<SaveSnap>();

        
        public LocationSnap(MonoBehaviour behaviour, IEnumerable<SaveSnap> items = null) : base(behaviour)
        {
            if (items != null)
                Items = items.ToArray();
        }

        public LocationSnap(string id, IEnumerable<SaveSnap> items = null) : base(id)
        {
            if (items != null)
                Items = items.ToArray();
        }

        
        public LocationSnap UpdateSnap(Func<SaveSnap, bool> itemsSelector)
        {
            return new LocationSnap(Id, Items.Where(itemsSelector.Invoke).ToArray());
        }
    }

    [System.Serializable]
    public class WorldSnap : SaveSnap
    {
        public SaveSnap[] Locations { get; private set; } = Array.Empty<SaveSnap>();

        
        public WorldSnap(MonoBehaviour behaviour, IEnumerable<SaveSnap> locations = null) : base(behaviour)
        {
            if (locations != null)
                Locations = locations.ToArray();
        }
        
        public WorldSnap(string id, IEnumerable<SaveSnap> locations = null) : base(id)
        {
            if (locations != null)
                Locations = locations.ToArray();
        }


        public void RemoveLocation(string locationId)
        {
            Locations = Locations.Where(location => !location.Id.Equals(locationId)).ToArray();
        }
    }
}