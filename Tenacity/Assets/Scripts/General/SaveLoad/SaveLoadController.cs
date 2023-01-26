using Tenacity.General.SaveLoad.Data;
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
        [SerializeField] private string _databaseFile;

        private SnapshotDatabase _database = new();
        private SaveSnapshot _snapshot = new();
        private string _workDatabasePath;

        public IReadOnlyList<SaveSnapshot> Snapshots
        {
            get => (_workDatabasePath == null) ? null : _database.Snapshots;
        }
        

        private void Start()
        {
            _workDatabasePath = Application.persistentDataPath + _databasePath;

            using var stream = GetDatabaseStream();
            LoadSnapshots(stream);
        }


        private FileStream GetDatabaseStream()
        {
            if (!Directory.Exists(_workDatabasePath + "/"))
                Directory.CreateDirectory(_workDatabasePath + "/");
            if (!File.Exists(_workDatabasePath + "/" + _databaseFile))
                File.Create(_workDatabasePath + "/" + _databaseFile);
            
            return new FileStream(_workDatabasePath + "/" + _databaseFile, 
                FileMode.OpenOrCreate, 
                FileAccess.ReadWrite, 
                FileShare.None);
        }

        private void LoadSnapshots(FileStream stream)
        {
            try
            {
                var importer = new BinaryImporter(_workDatabasePath, _databaseFile);
                _database = new SnapshotDatabase(importer.Import(stream));
                
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured during loading: {e.Message}");
            }
        }

        private void SaveSnapshots(FileStream stream)
        {
            try
            {
                var exporter = new BinaryExporter(_workDatabasePath, _databaseFile);

                exporter.Export(stream, new SnapshotDatabase(_database));
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured during saving: {e.Message}");
            }
        }


        public void SaveSnapshot()
        {
            if (_snapshot == null)
                return;

            try
            {
                using var stream = GetDatabaseStream();
                
                LoadSnapshots(stream);
                _database.Snapshots.Add(_snapshot);
                SaveSnapshots(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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
            _database.Snapshots.Remove(snapshot);
            
            using var stream = GetDatabaseStream();
            SaveSnapshots(stream);
        }

        public SaveSnapshot GetSnapshot(int index)
        {
            return ((index < 0) || (index >= _database.Snapshots.Count)) ? null : _database.Snapshots[index];
        }

        public SaveSnapshot GetLastSnapshot()
        {
            return _database.Snapshots[^1];
        }
        
        public void RemoveFromSnapshot(string id)
        {
            _snapshot.Data.RemoveWhere(snap => snap.Id.Equals(id));
        }
        
        public void AddToSnapshot(SaveSnap saveData)
        {
            _snapshot.Data.Add(saveData);
        }
    }
}