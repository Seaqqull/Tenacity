using Tenacity.UI.Additional;
using Tenacity.Managers;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;


namespace Tenacity.UI.Menus
{
    public class SettingsMenu : SingleMenu<SettingsMenu>
    {
        [System.Serializable]
        public enum SwitchDirection { Decrease = -1, Increase = 1 }
        
        [SerializeField] private int _selectedTab;
        [SerializeField] private SwitchableButton[] _settingTabs;
        [Space]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectsSlider;
        [SerializeField] private Slider _timeScaleSlider;
        [SerializeField] private TMP_Dropdown _languageSelect;
        [Header("BatteryMode")]
        [SerializeField] private TMP_Dropdown _batteryModeSelect;

        private void Start()
        {
            _musicSlider.value = Utility.Methods.FloatHelper.Map(
                AudioManager.Instance.Music, 
                Utility.Constants.Audio.MIN_LOUDNESS, 
                Utility.Constants.Audio.MAX_LOUDNESS,
                _musicSlider.minValue,
                _musicSlider.maxValue
            );
            
            _effectsSlider.value = Utility.Methods.FloatHelper.Map(
                AudioManager.Instance.Effects, 
                Utility.Constants.Audio.MIN_LOUDNESS, 
                Utility.Constants.Audio.MAX_LOUDNESS,
                _effectsSlider.minValue,
                _effectsSlider.maxValue
            );
            
            _timeScaleSlider.value = Utility.Methods.FloatHelper.Map(
                StorageManager.Instance.TimeScale, 
                Utility.Constants.Game.TIME_SCALE_MIN, 
                Utility.Constants.Game.TIME_SCALE_MAX,
                _timeScaleSlider.minValue,
                _timeScaleSlider.maxValue
            );
            
            // Initialization of languages
            _languageSelect.options = LocalizationManager.Instance
                .Locales.Select(locale => new TMP_Dropdown.OptionData(locale.LocaleName)).ToList();
            _languageSelect.SetValueWithoutNotify(LocalizationManager.Instance.SelectedLocaleIndex);

            // Initialization of battery
            _batteryModeSelect.options =
                Utility.Constants.Framerate.ALLOWED_FRAMERATE.Select(framerate =>
                    new TMP_Dropdown.OptionData(framerate.ToString())).ToList();
            _batteryModeSelect.value = Utility.Constants.Framerate.ALLOWED_FRAMERATE.
                IndexOf(StorageManager.Instance.TargetFramerate);
            
            // Activate tab
            OnTabClick(_settingTabs[_selectedTab]);
        }

        
        public void SwitchTimeValue(int valueChange)
        {
            var direction = (SwitchDirection)valueChange; 
            if ((direction == SwitchDirection.Decrease) && (_timeScaleSlider.value > _timeScaleSlider.minValue))
            {
                _timeScaleSlider.value--;
            }
            else if ((direction == SwitchDirection.Increase) && (_timeScaleSlider.value < _timeScaleSlider.maxValue))
            {
                _timeScaleSlider.value++;
            }
        }

        public void SwitchMusicValue(int valueChange)
        {
            var direction = (SwitchDirection)valueChange; 
            if ((direction == SwitchDirection.Decrease) && (_musicSlider.value > _musicSlider.minValue))
            {
                _musicSlider.value--;
            }
            else if ((direction == SwitchDirection.Increase) && (_musicSlider.value < _musicSlider.maxValue))
            {
                _musicSlider.value++;
            }
        }
        
        public void SwitchEffectsValue(int valueChange)
        {
            var direction = (SwitchDirection)valueChange; 
            if ((direction == SwitchDirection.Decrease) && (_effectsSlider.value > _effectsSlider.minValue))
            {
                _effectsSlider.value--;
            }
            else if ((direction == SwitchDirection.Increase) && (_effectsSlider.value < _effectsSlider.maxValue))
            {
                _effectsSlider.value++;
            }
        }

        public void SetTimeScale(float scale)
        {
            StorageManager.Instance.UpdateTimeScale(Utility.Methods.FloatHelper.Map(
                scale, 
                _timeScaleSlider.minValue,
                _timeScaleSlider.maxValue,
                Utility.Constants.Game.TIME_SCALE_MIN, 
                Utility.Constants.Game.TIME_SCALE_MAX
            ));
        }

        public void SetMusicVolume(float loudness)
        {
             AudioManager.Instance.UpdateMusicVolume(Utility.Methods.FloatHelper.Map(
                 loudness, 
                 _musicSlider.minValue,
                 _musicSlider.maxValue,
                 Utility.Constants.Audio.MIN_LOUDNESS, 
                 Utility.Constants.Audio.MAX_LOUDNESS
             ));
        }

        public void SetEffectsVolume(float loudness)
        {
            AudioManager.Instance.UpdateEffectsVolume(Utility.Methods.FloatHelper.Map(
                loudness, 
                _effectsSlider.minValue,
                _effectsSlider.maxValue,
                Utility.Constants.Audio.MIN_LOUDNESS, 
                Utility.Constants.Audio.MAX_LOUDNESS
            ));
        }
        
        public void OnTabClick(SwitchableButton clickedTab)
        {
            if (clickedTab.Selected) return;

            foreach (var tab in _settingTabs)
            {
                if (tab.Selected)
                {
                    tab.SwitchSelection();
                    break;
                }
            }
            clickedTab.SwitchSelection();
        }

        
        public void OnChangeLocale(int localeIndex)
        {
            LocalizationManager.Instance.SelectLocale(localeIndex);
            PlayerPrefsManager.Instance.SetInt(Utility.Constants.PlayerPrefs.LOCALE, localeIndex);
        }
        
        public void OnChangeBatteryMode(int modeIndex)
        {
            var framerate = Utility.Constants.Framerate.FramerateFromIndex(modeIndex);
            
            UpdateFrameRate(framerate);
            StorageManager.Instance.UpdateTargetFramerate(framerate);
        }
        
        public static void UpdateFrameRate(int targetFramerate)
        {
            QualitySettings.vSyncCount = Application.isMobilePlatform ? 0 : 1; // 1 for Standalone - if component is hidden on that platform 
            if (Application.isMobilePlatform)
            {
                Application.targetFrameRate = targetFramerate;
            }
#if UNITY_EDITOR
            else
            {
                Application.targetFrameRate = targetFramerate;
            }
#endif
        }
        
        public static void SetFrameRate()
        {
            var savedFramerate = StorageManager.GetSavedFramerate();
            UpdateFrameRate(savedFramerate);
        }
    }
}