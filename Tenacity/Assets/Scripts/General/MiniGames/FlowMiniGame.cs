using Tenacity.General.MiniGames.Additional;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;


namespace Tenacity.General.MiniGames
{
    public class FlowMiniGame : MiniGame
    {
        [Header("Additional")]
        [SerializeField] private int _switchersCount;
        [SerializeField] private Switcher _switcher;
        [SerializeField] private GameObject _switcherParent;
        [Header("Controlling")] 
        [SerializeField] [Range(0.1f, 1.0f)] private float _uiUpdateTime = 0.1f;
        [SerializeField] private Slider _frontSlider;
        [SerializeField] private Image _frontImage;
        [SerializeField] private Slider _backSlider;
        [SerializeField] private Image _backImage;
        [Header("Colors")] 
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _targetColor;
        [SerializeField] private Color _overflowColor;
        [Header("Generation")] 
        [SerializeField] private int _minSwitchersCount;
        [SerializeField] private int _maxSwitchersCount;
        [SerializeField] private int _minSwitchValue;
        [SerializeField] private int _maxSwitchValue;

        private Coroutine _updateUICoroutine;
        private Switcher[] _switchers;
        
        private float _requiredRelativeFlow;
        private int _requiredFlow;
        private int _overallFlow;
        private int _currentFlow;

        public bool ActionAllowec
        {
            get => (_updateUICoroutine == null);
        }
        
        
        protected void Awake()
        {
            // Flow
            var switchersPower = Enumerable.Range(0, _switchersCount)
                .Select(power => Random.Range(_minSwitchValue, _maxSwitchValue)).ToArray();
            _overallFlow = switchersPower.Sum(power => power);

            // Switchers power
            var proposedSwitchersCount = Random.Range(_minSwitchersCount, _maxSwitchersCount + 1);
            var selectedSwitchers = new HashSet<int>();
            while (selectedSwitchers.Count != proposedSwitchersCount)
                selectedSwitchers.Add(Random.Range(0, _switchersCount));
            _requiredFlow = selectedSwitchers.Select(switcherIndex => switchersPower[switcherIndex]).Sum(power => power);
            _requiredRelativeFlow = ((float)_requiredFlow / _overallFlow);

            // Switchers initialization
            _switchers = new Switcher[_switchersCount];
            for (int i = 0; i < _switchersCount; i++)
            {
                _switchers[i] = Instantiate(_switcher, _switcherParent.transform);
                var switcherPower = switchersPower[i];
                
                _switchers[i].SwitchAllowed += () => ActionAllowec;
                _switchers[i].OnSwitch += (enabled) => {
                    OnSwitch(enabled ? switcherPower : -switcherPower);
                };
            }
        }

        private void Start()
        {
            _backSlider.value = ((float)_requiredFlow / _overallFlow);
            _backImage.color = _targetColor;
            
            _frontSlider.value = 0.0f;
        }

        
        private void OnSwitch(int power)
        {
            _updateUICoroutine = StartCoroutine(UpdateUIRoutine(power));
        }

        protected override void Validate()
        {
            if (_currentFlow == _requiredFlow)
            {
                Debug.Log("Win");
                Succeed();
            }
        }
        
        private IEnumerator UpdateUIRoutine(int flowChange)
        {
            var desiredFlow = _currentFlow + flowChange;
            var passedTime = 0.0f;
            
            do
            {
                passedTime += Time.deltaTime;
                var currentFlow = Mathf.SmoothStep(_currentFlow, desiredFlow, (passedTime / _uiUpdateTime));
                var relativeCurrentFlow = (currentFlow / _overallFlow);
                
                _frontSlider.value = relativeCurrentFlow;
                _frontImage.color = (relativeCurrentFlow > _requiredRelativeFlow) ? _overflowColor : 
                        Color.Lerp(_defaultColor, _targetColor, (currentFlow / _requiredFlow));
                yield return null;
            } while (passedTime < _uiUpdateTime);

            _currentFlow = desiredFlow;
            _frontSlider.value = ((float)_currentFlow / _overallFlow);
            
            _updateUICoroutine = null;
            Validate();
        }

    }
}