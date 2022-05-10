using System.Runtime.Serialization.Formatters.Binary;
using Tenacity.General.SaveLoad.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Tenacity.Base;
using UnityEngine;
using System.IO;
using System;


namespace Tenacity.General.SaveLoad
{
    public class SaveLoadController : SingleBehaviour<SaveLoadController>
    {
        [SerializeField] private string _databasePath;

        private List<Data.SaveSnapshot> _snapshots = new();
        private SaveSnapshot _snapshot = new();
        private string _database;

        public IReadOnlyList<SaveSnapshot> Snapshots
        {
            get => (_database == null) ? null : _snapshots;
        }
        

        private void Start()
        {
            _database = Application.persistentDataPath + _databasePath;
            LoadSnapshots();
        }


        private void LoadSnapshots()
        {
            if (!File.Exists(_database)) return;

            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_database, FileMode.Open);
            stream.Position = 0;
            
            if (stream.Length != 0)
                _snapshots = (formatter.Deserialize(stream) as List<Data.SaveSnapshot>);
            stream.Close();
        }

        private void SaveSnapshots()
        {
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_database, FileMode.Create);
            
            formatter.Serialize(stream, _snapshots);
            stream.Close();
            ClearSnapshot();
        }


        public void SaveSnapshot()
        {
            if (_snapshot == null)
                return;

            LoadSnapshots();
            _snapshots.Add(_snapshot);
            
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_database, FileMode.Create);
            
            formatter.Serialize(stream, _snapshots);
            stream.Close();
            ClearSnapshot();
        }

        public void ClearSnapshot()
        {
            _snapshot = new SaveSnapshot();
            _snapshot.Title = DateTime.Now.ToBinary().ToString("HH:mm:ss MM/dd/yyyy");
        }

        public void SetSnapshotTitle(string title)
        {
            _snapshot.Title = title;
        }

        public void RemoveSnapshot(SaveSnapshot snapshot)
        {
            _snapshots.Remove(snapshot);
            SaveSnapshots();
        }

        public SaveSnapshot GetSnapshot(int index)
        {
            return ((index < 0) || (index >= _snapshots.Count)) ? null : _snapshots[index];
        }

        public SaveSnapshot GetLastSnapshot()
        {
            return _snapshots[^1];
        }
        
        public void RemoveFromSnapshot(int id)
        {
            _snapshot.Data.RemoveWhere(snap => snap.Id == id);
        }
        
        public void AddToSnapshot(SaveSnap saveData)
        {
            _snapshot.Data.Add(saveData);
        }
    }
}