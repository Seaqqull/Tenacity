using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


namespace Tenacity.UI.Additional
{
    public class SnapshotItem : SwitchableButton
    {
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
        public DateTime Date
        {
            set => _date.text = value.ToString(_dateFormat);
        }


        public override void SetSelection(bool selection)
        {
            if (_selected == selection) return;

            base.SetSelection(selection);
            _onChangeState?.Invoke(this);
        }
    }
}
