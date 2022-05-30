using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


namespace Tenacity.UI.Additional
{
    public class SnapshotItem : MonoBehaviour
    {
        [SerializeField] private bool _selected;
        [Header("UI")] 
        [SerializeField] private GameObject _ordinalBackground;
        [SerializeField] private GameObject _selectedBackground;
        [Space] 
        [SerializeField] private TMP_Text _screenText;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _date;
        [Space]
        [SerializeField] private string _dateFormat;
        [Space]
        [SerializeField] private Slider _ending;
        [SerializeField] private Slider _progress;

        private Action<SnapshotItem> _onChangeState;
        
        public event Action<SnapshotItem> OnChangeState
        {
            add { _onChangeState += value; }
            remove { _onChangeState -= value; }
        }
        public string ScreenText
        {
            set => _screenText.text = value;
        }
        public string TitleText
        {
            set => _title.text = value;
        }
        public bool Selected
        {
            get => _selected;
        }
        public DateTime Date
        {
            set => _date.text = value.ToString(_dateFormat);
        }


        private void Awake()
        {
            SwitchButtons(_selected);
        }


        private void SwitchButtons(bool enable)
        {
            _selectedBackground.SetActive(enable);
            _ordinalBackground.SetActive(!enable);
        }

        
        public void SwitchSelection()
        {
            SetSelection(!_selected);
        }

        public void SetSelection(bool selection)
        {
            if (_selected == selection) return;

            _selected = selection;
            SwitchButtons(_selected);
            _onChangeState?.Invoke(this);
        }
    }
}
