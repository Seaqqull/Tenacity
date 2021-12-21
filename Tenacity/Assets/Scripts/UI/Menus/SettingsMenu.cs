using Tenacity.Managers;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;


namespace Tenacity.UI.Menus
{
    public class SettingsMenu : SingleMenu<SettingsMenu>
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectsSlider;
        [SerializeField] private TMP_Dropdown _languageSelect;


        private void Start()
        {
            _effectsSlider.value = Utility.Methods.FloatHelper.Map(
                AudioManager.Instance.Effects, 
                Utility.Constants.Audio.MIN_LOUDNESS, 
                Utility.Constants.Audio.MAX_LOUDNESS,
                _effectsSlider.minValue,
                _effectsSlider.maxValue
            );
            
            _musicSlider.value = Utility.Methods.FloatHelper.Map(
                AudioManager.Instance.Music, 
                Utility.Constants.Audio.MIN_LOUDNESS, 
                Utility.Constants.Audio.MAX_LOUDNESS,
                _musicSlider.minValue,
                _musicSlider.maxValue
            );
            
            // Initialization of languages
            _languageSelect.options = LocalizationManager.Instance
                .Locales.Select(locale => new TMP_Dropdown.OptionData(locale.LocaleName)).ToList();
            _languageSelect.SetValueWithoutNotify(LocalizationManager.Instance.SelectedLocaleIndex);
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

        public void OnChangeLocale(int localeIndex)
        {
            LocalizationManager.Instance.SelectLocale(localeIndex);
            PlayerPrefsManager.Instance.SetInt(Utility.Constants.PlayerPrefs.LOCALE, localeIndex);
        }
    }
}