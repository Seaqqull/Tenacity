using Tenacity.General.SaveLoad.Data;
using System.Security.Cryptography;
using System.Collections;
using System.Text;
using UnityEngine;


namespace Tenacity.General.SaveLoad.Implementation
{
    public class LocationItem : MonoBehaviour, ISavable
    {
        private bool _exists = true;
        
        public string Id { get; private set; }
        public bool Exists
        {
            get => _exists;
            set
            {
                _exists = value;
                Location.Instance.AddItem(this);
            }
        }


        private void Awake()
        {
            if (Id == null) InitializeId();
        }

        protected virtual void Start()
        {
            StartCoroutine(ConnectWithLocationRoutine());
        }

        private void OnDestroy()
        {
            if (Location.Instance != null)
                Location.Instance.RemoveItem(this);
        }

        
        private IEnumerator ConnectWithLocationRoutine()
        {
            yield return null;
            
            if (Location.Instance != null)
                Location.Instance.AddItem(this);
        }

        private void InitializeId()
        {
            Id = SaveSnap.GetHash(this);
        }


        public void UpdateItemState()
        {
            Exists = _exists;
        }

        #region Savable
        public virtual SaveSnap MakeSnap()
        {
            return new LocationItemSnap(Id, _exists);
        }

        public virtual void FromSnap(SaveSnap data)
        {
            if (Id == null) InitializeId();
            
            var itemData = data as LocationItemSnap;
            if ((itemData == null) || !itemData.Id.Equals(Id))
                return;
            
            
            _exists = itemData.Exists;
            Location.Instance.AddItem(this);

            if (!_exists)
                Destroy(gameObject);
        }
        #endregion
        
        
        private static string GetHash(SHA256 hashAlgorithm, string input)
        {
            var inputData = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var hash = new StringBuilder();
            foreach (var data in inputData)
                hash.Append(data.ToString("x2"));

            return hash.ToString();
        }
    }
}