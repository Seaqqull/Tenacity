using UnityEngine;
using System;


namespace Tenacity.Managers
{
    public class StorageManager : Base.SingleBehaviour<StorageManager>
    {
        #region Constants
        private const string CAMERA_UI = "UICamera";
        #endregion

        private Vector3 _defaultPlayerPosition;
        private bool _updatePlayerData;
        
        public Camera MainCamera { get; private set; }
        public Camera UICamera { get; private set; }
        public float TimeScale { get; private set; }
        public float Time { get; private set; }


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
            
            // Player
            if (_updatePlayerData && (PlayerManager.Instance != null))
            {
                _updatePlayerData = false;
                PlayerManager.Instance.transform.position = _defaultPlayerPosition;
            }
        }


        public void UpdatePlayerPosition(Vector3 newPosition)
        {
            _updatePlayerData = true;
            _defaultPlayerPosition = newPosition;
        }

        public void UpdateTime(float time)
        {
            Time = time;
            if (EnvironmentManager.Instance != null)
                EnvironmentManager.Instance.GameTime = Time;
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