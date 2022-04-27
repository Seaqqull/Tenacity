using System.Collections;
using Tenacity.Base;
using UnityEngine;


namespace Tenacity.General.Sequence
{
    public abstract class SequentialExecutor : BaseMono
    {
        public enum SequenceType { None, OneWay, FrontAndBack }


        [SerializeField] protected float _absoluteTime = 1.0f;
        [SerializeField] [Range(0.0f, 0.5f)] protected float _timeBetween = 0.1f;
        [SerializeField] protected SequenceType _type;
        [SerializeField] protected bool _runWhenEnable = true;
        [SerializeField] protected bool _isInfinite;
        // Used this class to increase efficiency of performing different changes on object transform
        [SerializeField] protected Transform _objectToChange;

        protected Coroutine _coroutine;
        protected float _progress;
        protected float _timeHalf;
        protected float _elapsed;


        protected override void Awake()
        {
            base.Awake();

            if (_objectToChange == null)
                _objectToChange = Transform;
            PackData();
        }

        protected virtual void Start()
        {
            ResetSequence();
        }

        protected void FixedUpdate()
        {
            if (_timeBetween != 0.0f) return;

            if (_elapsed < _absoluteTime)
                CalculateSequence();
            else if(_isInfinite)
                ResetSequence();
        }


        protected virtual void OnEnable()
        {
            if (_runWhenEnable && _timeBetween != 0.0f)
                _coroutine = StartCoroutine(OnSequence());
        }

        protected virtual void OnDisable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            UnPackData();
        }


        protected void ResetSequence()
        {
            _progress = 0.0f;
            _elapsed = 0.0f;
            
            if (_type == SequenceType.FrontAndBack)
                _timeHalf = _absoluteTime / 2.0f;
        }

        protected IEnumerator OnSequence()
        {
            var wait = (_timeBetween == 0.0f)? null : new WaitForSeconds(_timeBetween);

            do
            {
                _progress = 0.0f;
                _elapsed = 0.0f;
                
                if (_type == SequenceType.FrontAndBack)
                    _timeHalf = (_absoluteTime / 2.0f);


                while (_elapsed < _absoluteTime)
                {
                    CalculateSequence();
                    yield return wait;
                }
            } while (_isInfinite);

            _coroutine = null;
        }

        protected void CalculateSequence()
        {
            switch (_type)
            {
                case SequenceType.OneWay:
                    _progress = (_elapsed / _absoluteTime);
                    break;
                case SequenceType.FrontAndBack:
                    _progress = (_elapsed < _timeHalf) ? (_elapsed / _timeHalf) : (2.0f - (_elapsed / _timeHalf));
                    break;
#if UNITY_EDITOR
                case SequenceType.None:
                default:
                    Debug.Log("You should select sequence type for it's proper work.", GameObject);
                    break;
#endif
            }
            DoAction(_progress);

            _elapsed += Time.deltaTime;
        }


        protected abstract void PackData();
        protected abstract void UnPackData();
        protected abstract void DoAction(float progress);
    }
}