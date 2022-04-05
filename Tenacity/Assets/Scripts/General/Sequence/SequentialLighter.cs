using UnityEngine;


namespace Tenacity.General.Sequence
{
    public class SequentialLighter : SequentialExecutor
    {
        [SerializeField] private Light _light;
        [SerializeField] private Color _endColor;
        [SerializeField] private float _intensity;

        private float _originIntensity;
        private Color _originColor;
        
        
        protected override void PackData()
        {
            _originIntensity = _light.intensity;
            _originColor = _light.color;
        }

        protected override void UnPackData()
        {
            _light.intensity = _originIntensity;
            _light.color = _originColor;
        }

        protected override void DoAction(float progress)
        {
            _light.intensity = Mathf.Lerp(_originIntensity, _intensity, progress);
            _light.color = Color.Lerp(_originColor, _endColor, progress);
        }
    }
}
