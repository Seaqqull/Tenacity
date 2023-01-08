using Tenacity.General.SaveLoad.Data;
using Tenacity.Managers.Additional;
using System.Collections.Generic;
using Tenacity.General.SaveLoad;
using Tenacity.Base;
using System;


namespace Tenacity.Managers
{
    public class SaveLoadManager : SingleBehaviour<SaveLoadManager>, ISavable
    {
        private List<ISavable> _savables = new();
        private Action _onSave;

        public IReadOnlyList<SaveSnapshot> Snapshots
        {
            get
            {
                return SaveLoadController.Instance.Snapshots;
            }
        }
        public string Id { get; private set; }
        public event Action OnSave
        {
            add { _onSave += value; }
            remove { _onSave -= value; }
        }


        private void Start()
        {
            Id = SaveSnap.GetHash(this);
        }


        private void FromSnapshot(SaveSnapshot snapshot, SaveLoadSnap snap)
        {
            FromSnap(snap);

            if (snap != null)
            {
                SceneManager.Instance.LoadLevel(snap.SceneIndex, snapshot.Title, () =>
                {
                    foreach (var savable in _savables)
                    {
                        foreach (var snap in snapshot.Data)
                        {
                            savable.FromSnap(snap);
                        }
                    }
                });
            }
        }
        
        
        public void Save()
        {
            SaveLoadController.Instance.ClearSnapshot();
            
            SaveLoadController.Instance.SetSnapshotTitle(SceneManager.Instance.LevelName);
            SaveLoadController.Instance.AddToSnapshot(MakeSnap());
            foreach (var savable in _savables)
            {
                SaveLoadController.Instance.AddToSnapshot(savable.MakeSnap());
            }
            SaveLoadController.Instance.SaveSnapshot();
            
            _onSave?.Invoke();
        }

        public void Load()
        {
            var snapshot = SaveLoadController.Instance.GetLastSnapshot();
        }

        public void Load(int snapshotIndex)
        {
            var snapshot = SaveLoadController.Instance.GetSnapshot(snapshotIndex);
            if (snapshot == null) return;

            SaveLoadSnap snapData = null;
            foreach (var snap in snapshot.Data)
            {
                snapData = snap as SaveLoadSnap;
                if (snapData != null) break;
            }
            
            FromSnapshot(snapshot, snapData);
        }

        public void RemoveFromSavable(string id)
        {
            var savableIndex = _savables.FindIndex(savable => savable.Id.Equals(id));
            if (savableIndex != -1)
                _savables.RemoveAt(savableIndex);
        }
        
        public void AddToSavable(ISavable savable)
        {
            if (!_savables.Contains(savable))
                _savables.Add(savable);
        }
        
        public void RemoveFromSavable(ISavable savable)
        {
            _savables.Remove(savable);
        }
        
        public void DeleteSnapshot(int snapshotIndex)
        {
            SaveLoadController.Instance.RemoveSnapshot(Snapshots[snapshotIndex]);
        }
        
        #region Saves
        public SaveSnap MakeSnap()
        {
            var saveData = new SaveLoadSnap(this)
            {
                TargetFramerate = StorageManager.Instance.TargetFramerate,
                GameTimeScale = EnvironmentManager.Instance.GameTimeScale, 
                GameTime = EnvironmentManager.Instance.GameTime,
                Date = DateTime.Now.ToBinary(),
                SceneIndex = SceneManager.Instance.LevelIndex,
                PlayerPosition = new SaveLoadSnap.Position(PlayerManager.Instance.transform.position)
            };
            saveData.AssignAccessor(() => SaveLoadManager.Instance != null);
            saveData.AssignGetter(() => SaveLoadManager.Instance);

            return saveData;
        }

        public void FromSnap(SaveSnap data)
        {
            var savedData = data as SaveLoadSnap;
            if (savedData == null)
                return;

            StorageManager.Instance.UpdateTime(savedData.GameTime);
            StorageManager.Instance.UpdateTimeScale(savedData.GameTimeScale);
            StorageManager.Instance.UpdatePlayerPosition(savedData.PlayerPosition);
            StorageManager.Instance.UpdateTargetFramerate(savedData.TargetFramerate);

            GameStorageUpdateManager.Instance.UpdateGame(StorageManager.Instance);
        }
        #endregion
    }
}