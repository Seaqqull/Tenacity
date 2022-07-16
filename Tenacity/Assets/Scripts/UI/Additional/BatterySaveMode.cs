using UnityEngine;


namespace Tenacity.UI.Additional
{
    public class BatterySaveMode : MonoBehaviour
    {
        private bool _modeOn;
        
        
        public void OnBatteryModeClick(bool isOn)
        {
            _modeOn = isOn;
            SetFrameRate(_modeOn);
        }


        public static void SetFrameRate(bool isOn)
        {
            QualitySettings.vSyncCount = Application.isMobilePlatform ? 0 : (isOn) ? 1 : 0; 
            if (Application.isMobilePlatform)
            {
                Application.targetFrameRate = isOn ? 30 : 60;
            }
#if UNITY_EDITOR
            else
            {
                Application.targetFrameRate = isOn ? Screen.currentResolution.refreshRate : -1;
            }
#endif
        }
    }
}
