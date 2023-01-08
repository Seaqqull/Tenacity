using Tenacity.General.SaveLoad.Data;
using System.Collections.Generic;
using Tenacity.Cards.Inventory;
using Tenacity.Managers;
using UnityEngine;
using System.Linq;


namespace Tenacity.General.SaveLoad.Implementation
{
    [System.Serializable]
    public class NPCSnap : LocationItemSnap
    {
        public int[] ItemIds { get; private set; }
        public int Currency { get; private set; }


        public NPCSnap(MonoBehaviour behaviour, int currency, IEnumerable<int> items) : base(behaviour)
        {
            ItemIds = items.ToArray();
            Currency = currency;
        }
        
        public NPCSnap(string id, int currency, IEnumerable<int> items) : base(id)
        {
            ItemIds = items.ToArray();
            Currency = currency;
        }
    }
    
    public class NPCLocationItem : LocationItem
    {        
        [SerializeField] private EntityInventory _defaultInventory;
        [SerializeField] private EntityInventory _inventory;


        protected override void Start()
        {
            base.Start();
            
            _inventory.InitializeInventory(_defaultInventory.Currency, _defaultInventory.Items);
        }
        
        
        #region Savable
        public override SaveSnap MakeSnap()
        {
            return new NPCSnap(Id, _inventory.Currency, _inventory.Items.Select(item => item.Id).ToList());
        }

        public override void FromSnap(SaveSnap data)
        {
            var playerSnap = data as NPCSnap;
            if (playerSnap == null) return;

            _inventory.InitializeInventory(playerSnap.Currency, ItemsDatabaseManager.Instance.GetItems(playerSnap.ItemIds));
        }
        #endregion
    }
}
