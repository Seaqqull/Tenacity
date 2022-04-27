using UnityEngine;


namespace Tenacity.General.Sequence
{
    public class SequentialTranslator : SequentialExecutor
    {
        [SerializeField] private Vector3 _beginShift;
        [SerializeField] private Vector3 _endShift;

        private Vector3 _originTransform;
        private Vector3 _currentShift;

        
        protected override void PackData()
        {
            _originTransform = _objectToChange.localPosition;
        }

        protected override void UnPackData()
        {
            _objectToChange.localPosition = _originTransform;
        }

        protected override void DoAction(float progress)
        {
            _currentShift = Vector3.Lerp(_beginShift, _endShift, progress);

            _objectToChange.localPosition = (_originTransform + _currentShift);
        }
    }
}