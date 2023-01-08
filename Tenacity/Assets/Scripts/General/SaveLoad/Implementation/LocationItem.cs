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
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}