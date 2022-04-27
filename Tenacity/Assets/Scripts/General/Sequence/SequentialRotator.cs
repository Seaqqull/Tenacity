using UnityEngine;


namespace Tenacity.General.Sequence
{
    public class SequentialRotator : SequentialExecutor
    {
        [SerializeField] private Vector3 _rotateByAngle;

        private Quaternion _originRotation;
        private Quaternion _currentRotation;


        protected override void PackData()
        {
            _originRotation = _objectToChange.localRotation;
        }

        protected override void UnPackData()
        {
            _objectToChange.localRotation = _originRotation;
        }        

        protected override void DoAction(float progress)
        {
            _currentRotation = new Quaternion();
            _currentRotation.eulerAngles = 
                Vector3.Lerp(_originRotation.eulerAngles, _rotateByAngle, progress);
            
            _objectToChange.localRotation = _currentRotation;
        }
    }
}