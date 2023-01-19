using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace Tenacity.Battles.Views
{
    public class MaskableValueView : MonoBehaviour
    {
        [SerializeField] private int _minAmount;
        [SerializeField] private int _maxAmount;
        [SerializeField] private int _amount;
        [Space] 
        [SerializeField] private bool _textWithMax;
        [Space] 
        [SerializeField] private Image _fillImage;
        [SerializeField] private Color _colorMin;
        [SerializeField] private Color _colorMax;
        [SerializeField] private TMP_Text _text;
        

        private void ClampAmount()
        {
            if (_amount < _minAmount)
                _amount = _minAmount;
            else if ((_maxAmount > 0) && (_amount > _maxAmount))
                _amount = _maxAmount;
        }
        
        
        public void FillAmount(int value, int maxValue)
        {
            _maxAmount = (maxValue <= 0) ? _maxAmount : maxValue;
            _amount = value;
            ClampAmount();
                
            _text.text = _textWithMax ? $"{_amount}/{_maxAmount}" : $"{_amount}";
            
            var progress = ((float) _amount) / _maxAmount;
            _fillImage.color = Color.Lerp(_colorMin, _colorMax, Mathf.SmoothStep(0.0f, 1.0f, progress));
            _fillImage.fillAmount = (maxValue <= 0) ? 1.0f : progress;
        }
    }
}