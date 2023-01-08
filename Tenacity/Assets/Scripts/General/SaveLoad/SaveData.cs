using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;


namespace Tenacity.General.SaveLoad.Data
{
    [System.Serializable]
    public abstract class SaveSnap : IEqualityComparer<SaveSnap>
    {
        private Func<MonoBehaviour> _monoGetter;
        private Func<bool> _isMonoAccessible;
        public string Id;
        
        public MonoBehaviour GetMono
        {
            get { return _monoGetter?.Invoke(); }
        }
        public bool MonoAccessible
        {
            get { return _isMonoAccessible?.Invoke() ?? true; }
        }

        
        public SaveSnap(MonoBehaviour behaviour)
        {
            var gameObject = behaviour.gameObject;
            var position = gameObject.transform.position;
            var name = gameObject.name;
            
            Id = GetHash(SHA256.Create(), name + position.ToString("F2"));
        }

        public SaveSnap(string id)
        {
            Id = id;
        }


        public void AssignAccessor(Func<bool> isMonoAccessible)
        {
            _isMonoAccessible = isMonoAccessible;
        }

        public void AssignGetter<T>(Func<T> monoGetter) where T : MonoBehaviour, ISavable
        {
            _monoGetter = monoGetter;
        }


        #region Comparison
        public int GetHashCode(SaveSnap obj)
        {
            return obj.Id.GetHashCode();
        }
        
        public bool Equals(SaveSnap x, SaveSnap y)
        {
            if ((x == null) && (y == null))
                return true;
            if ((x == null) || (y == null))
                return false;
            return (x.Id == y.Id);
        }
        #endregion
        
        
        public static string GetHash(MonoBehaviour behaviour)
        {
            var gameObject = behaviour.gameObject;
            var position = gameObject.transform.position;
            var name = gameObject.name;
            
            return GetHash(SHA256.Create(), name + position.ToString("F2"));
        }
        
        public static string GetHash(SHA256 hashAlgorithm, string input)
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

    [System.Serializable]
    public class SaveSnapshot
    {
        public string Title;
        public HashSet<SaveSnap> Data = new();
    }
    
    [System.Serializable]
    public class SnapshotDatabase : ISerializable
    {
        public List<SaveSnapshot> Snapshots { get; set; } = new ();

        
        public SnapshotDatabase()
        {
            Snapshots = new List<SaveSnapshot>();
        }
        
        public SnapshotDatabase(IEnumerable<SaveSnapshot> snapshots)
        {
            Snapshots = new List<SaveSnapshot>(snapshots);
        }
        
        public SnapshotDatabase(SnapshotDatabase database) : this(database.Snapshots) { }

        
        public SnapshotDatabase(SerializationInfo info, StreamingContext context)
        {
            Snapshots = (List<SaveSnapshot>) info.GetValue(nameof(Snapshots), typeof(List<SaveSnapshot>));
        }
        

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Snapshots), Snapshots, typeof(List<SaveSnapshot>));
        }
    }
}
