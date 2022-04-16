using UnityEngine;


namespace Tenacity.General.Weather
{
    [CreateAssetMenu(menuName = "Weather/Lighting/DayNightCycle", fileName = "DayNightCycle", order = 0)]
    public class DayNightCycleSO : ScriptableObject
    {
        public Gradient DayNightColor;
        [Header("Day")]
        public float DayIntensity;
        public float DayRotationAngle;
        public AnimationCurve DayToNightTransition;
        [Header("Night")]
        public float NightIntensity;
        public float NightRotationStepAngle;
        public AnimationCurve NightToDayTransition;
    }
}
