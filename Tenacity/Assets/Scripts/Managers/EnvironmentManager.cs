using Tenacity.General.Weather;
using Tenacity.Base;
using UnityEngine;
using System;
using TMPro;


namespace Tenacity.Managers
{
    public class EnvironmentManager : SingleBehaviour<EnvironmentManager>
    {
        #region Constants
        private const int SECONDS_IN_DAY = 86400;
        private const int HALF_SECONDS_IN_DAY = (int)(SECONDS_IN_DAY * 0.5f);
        private const string TIME_FORMAT = "{0:D2}:{1:D2}:{2:D2}";
        #endregion
        
        [SerializeField] [Range(0.0f, SECONDS_IN_DAY)] private float _gameTime;
        [SerializeField] private TMP_Text _timeText;
        [field: SerializeField] [field: Range(0.0f, 10000)] public float GameTimeScale { get; set; } = 1.0f;
        [Header("Lighting")]
        [SerializeField] private PolyverseSkies _sky;
        [SerializeField] private Light _light;
        [SerializeField] private DayNightCycleSO _dayNightParameters;
        
        private float _actualGameTimeScale;
        private Transform _lightTransform;
        private float _lightRotationAngle;
        private float _lightRotationStep;

        public float GameTime
        {
            get => _gameTime;
            set
            {
                _gameTime = value;
                _gameTime %= SECONDS_IN_DAY;
                if (_gameTime < 0.0f)
                    _gameTime = SECONDS_IN_DAY - _gameTime;
            }
        }


        protected override void Awake()
        {
            base.Awake();

            _lightRotationStep = (_dayNightParameters.NightRotationStepAngle - _dayNightParameters.DayRotationAngle);
            _lightRotationAngle = _dayNightParameters.DayRotationAngle;
            _lightTransform = _light.transform;
        }

        private void Update()
        {
            var timeChangeStep = (Time.deltaTime * GameTimeScale);
            GameTime += timeChangeStep; 

            // Update light parameters
            bool lessThanHalfLimit = (_gameTime <= HALF_SECONDS_IN_DAY);
            float scaledTimeProgress = (lessThanHalfLimit) ? (_gameTime / HALF_SECONDS_IN_DAY) :
                (((_gameTime - HALF_SECONDS_IN_DAY)) / HALF_SECONDS_IN_DAY);
            float scaledDirectionalTimeProgress = (lessThanHalfLimit) ? scaledTimeProgress : (1.0f - scaledTimeProgress);
            float transitionProgress = (lessThanHalfLimit)
                ? _dayNightParameters.DayToNightTransition.Evaluate(scaledTimeProgress)
                : _dayNightParameters.NightToDayTransition.Evaluate(scaledTimeProgress);
            TimeSpan timeOfDay = TimeSpan.FromSeconds(Mathf.Abs(HALF_SECONDS_IN_DAY + _gameTime));
            
            string timeOfDayFormated = string.Format(TIME_FORMAT, 
                timeOfDay.Hours, 
                timeOfDay.Minutes, 
                timeOfDay.Seconds);
            _timeText.text = timeOfDayFormated;
            
            _sky.timeOfDay = scaledDirectionalTimeProgress;

            _lightRotationAngle += (_lightRotationStep * (timeChangeStep / HALF_SECONDS_IN_DAY));
            _lightRotationAngle %= 360.0f;
            
            _lightTransform.localRotation = Quaternion.Euler(_lightRotationAngle, 0.0f, 0.0f);
            _light.intensity = Mathf.Lerp(_dayNightParameters.DayIntensity, _dayNightParameters.NightIntensity, transitionProgress);
            _light.color = _dayNightParameters.DayNightColor.Evaluate(transitionProgress);
        }

        private void OnDestroy()
        {
            StorageManager.Instance.Time = _gameTime;
        }


        public void PauseGameTime()
        {
            _actualGameTimeScale = GameTimeScale;
            GameTimeScale = 0.0f;
        }

        public void ResumeGameTime()
        {
            GameTimeScale = _actualGameTimeScale;
        }


        public static float TimeFromDate(DateTime time)
        {
            var timeShift = time.TimeOfDay.Subtract(TimeSpan.FromSeconds(HALF_SECONDS_IN_DAY));
            return (float)timeShift.TotalSeconds;
        }
    }
}