using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using UnityEngine.Localization;
using Tenacity.Utility.Base;


namespace Tenacity.Managers
{
    public class LocalizationManager : SingleBehaviour<LocalizationManager>
    {
        private List<Locale> _locales;

        public IReadOnlyCollection<Locale> Locales
        {
            get { return _locales; }
        }
        public int SelectedLocaleIndex
        {
            get { return _locales.IndexOf(SelectedLocale); }
        }
        public Locale SelectedLocale
        {
            get { return LocalizationSettings.SelectedLocale; }
        }
        


        protected void Start()
        {
            LocalizationSettings.InitializationOperation.WaitForCompletion();
            _locales = LocalizationSettings.AvailableLocales.Locales;
        }


        public void SelectLocale(int localeIndex)
        {
            if (localeIndex < 0 || localeIndex >= _locales.Count)
                return;
            
            LocalizationSettings.Instance.SetSelectedLocale(_locales[localeIndex]);
            
        }
    }
}