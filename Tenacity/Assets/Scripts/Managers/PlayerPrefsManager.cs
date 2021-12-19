using Tenacity.Utility.Base;
using UnityEngine;


namespace Tenacity.Managers
{
    public class PlayerPrefsManager : SingleBehaviour<PlayerPrefsManager>
    {
        public void SaveData()
        {
            PlayerPrefs.Save();
        }
        
        public void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }
        
        public float GetFloat(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public int GetInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }
    }
}