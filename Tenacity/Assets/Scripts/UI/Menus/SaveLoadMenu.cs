using Tenacity.General.SaveLoad.Data;
using System.Collections.Generic;
using Tenacity.UI.Additional;
using Tenacity.Managers;
using UnityEngine.UI;
using UnityEngine;
using System;


namespace Tenacity.UI
{
    public class SaveLoadMenu : SingleMenu<SaveLoadMenu>
    {
        [Flags]
        public enum MenuState { None, Savable, Loadable }

        [SerializeField] private MenuState _state = (MenuState.Savable | MenuState.Loadable);
        [SerializeField] private SnapshotItem _snapshotPrefab;
        [SerializeField] private Transform _snapshotsParent;
        [Space] 
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _deleteButton;
        
        private List<SnapshotItem> _snapshots = new();
        private SnapshotItem _selectedSnapshot;

        
        protected override void Awake()
        {
            base.Awake();
            
            SetMenuState(_state);
        }

        private void Start()
        {
            ParseSnapshots();
        }

        private void OnEnable()
        {
            SaveLoadManager.Instance.OnSave += ParseSnapshots;
        }

        private void OnDisable()
        {
            SaveLoadManager.Instance.OnSave -= ParseSnapshots;
        }

        private void ParseSnapshots()
        {
            foreach (var snapshot in _snapshots)
                Destroy(snapshot.gameObject);
            _snapshots.Clear();

            var snapshotCounter = 1;
            foreach (var snapshot in SaveLoadManager.Instance.Snapshots)
                _snapshots.Add(VisualizeSnapshot(snapshot, snapshotCounter++));
        }

        private void SwitchButtons(bool enable)
        {
            _deleteButton.enabled = enable;
            _loadButton.enabled = enable;
        }

        private void OnSnapshotSelection(SnapshotItem interactedSnapshot)
        {
            if (_selectedSnapshot == null) // Nothing to do here
            {
                if (!interactedSnapshot.Selected)
                    return;
                _selectedSnapshot = interactedSnapshot;
            }
            else if ((interactedSnapshot != null) && interactedSnapshot.Selected) // Deselect current / Select new
            {
                var temporarySnapshot = _selectedSnapshot;
                _selectedSnapshot = null;
                temporarySnapshot.SetSelection(false);
                
                _selectedSnapshot = interactedSnapshot;
            }
            else // Deselect current
            {
                _selectedSnapshot = null;
            }

            SwitchButtons(_selectedSnapshot != null);
        }

        private SnapshotItem VisualizeSnapshot(SaveSnapshot snapshot, int index)
        {
            var snapshotItem = Instantiate(_snapshotPrefab, _snapshotsParent);
            snapshotItem.OnChangeState += OnSnapshotSelection;
            snapshotItem.TitleText = snapshot.Title;
            
            Managers.Additional.SaveLoadSnap snapshotData = null;
            foreach (var snap in snapshot.Data)
            {
                snapshotData = snap as Managers.Additional.SaveLoadSnap;
                if (snapshotData != null) break;
            }
            if (snapshotData == null)
                return snapshotItem;
            
            snapshotItem.Date = DateTime.FromBinary(snapshotData.Date);
            snapshotItem.ScreenText = index.ToString();
            return snapshotItem;
        }


        public void SaveData()
        {
            SaveLoadManager.Instance.Save();
            ParseSnapshots();
        }

        public void LoadData()
        {
            if (_selectedSnapshot == null)
                return;
            
            MenuManager.Instance.CloseAllMenus();
            SaveLoadManager.Instance.Load(_snapshots.IndexOf(_selectedSnapshot));
        }

        public void DeleteSnapshot()
        {
            if (_selectedSnapshot == null)
                return;

            var indexToRemove = _snapshots.IndexOf(_selectedSnapshot);
            SaveLoadManager.Instance.DeleteSnapshot(indexToRemove);

            _snapshots.RemoveAt(indexToRemove);
            Destroy(_selectedSnapshot.gameObject);
            
            
            OnSnapshotSelection(null);
            
            var snapshotCounter = 1;
            foreach (var snapshot in _snapshots)
                snapshot.ScreenText = (snapshotCounter++).ToString();
        }

        public void SetMenuState(MenuState state)
        {
            _state = state;

            _loadButton.enabled = _state.HasFlag(MenuState.Loadable);
            _saveButton.enabled = _state.HasFlag(MenuState.Savable);
        }
    }
}
