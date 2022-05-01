using UnityEngine;
using System;


namespace Tenacity.Managers
{
    public class StorageManager : Base.SingleBehaviour<StorageManager>
    {
        #region Constants
        private const string CAMERA_UI = "UICamera";
        #endregion

        public EnvironmentManager Environment { get; private set; }
        public Camera MainCamera { get; private set; }
        public Camera UICamera { get; private set; }
        public float TimeScale { get; private set; }
        public float Time { get; set; }


        private void Start()
        {
            Time = EnvironmentManager.TimeFromDate(DateTime.Now);
            TimeScale = PlayerPrefsManager.Instance.GetFloat(Utility.Constants.Game.TIME_SCALE,
                Utility.Constants.Game.TIME_SCALE_MIN);
        }
        

        public void UpdateGameObjects()
        {
            // Cameras
            UICamera = GameObject.FindGameObjectWithTag(CAMERA_UI)?.GetComponent<Camera>();
            MainCamera = Camera.main;
            
            // Environment
            if (EnvironmentManager.Instance != null)
            {
                EnvironmentManager.Instance.GameTimeScale = TimeScale;
                EnvironmentManager.Instance.GameTime = Time;   
            }
        }


        public void UpdateTimeScale(float scale)
        {
            PlayerPrefsManager.Instance.SetFloat(Utility.Constants.Game.TIME_SCALE, scale);
            TimeScale = scale;

            if (EnvironmentManager.Instance != null)
                EnvironmentManager.Instance.GameTimeScale = TimeScale;
        }
    }
}