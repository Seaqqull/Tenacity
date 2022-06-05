using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace Tenacity.UI
{
    public class ManaUI : MonoBehaviour
    {
        [SerializeField] private int _minAmount;
        [SerializeField] private int _maxAmount;
        [SerializeField] private int _amount;
        [Space] 
        [SerializeField] private Image _fillImage;
        [SerializeField] private Color _colorMin;
        [SerializeField] private Color _colorMax;
        [SerializeField] private TMP_Text _text;
        
        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                ClampAmount();
                
                _text.text = _amount.ToString();
                _fillImage.color = Color.Lerp(_colorMin, _colorMax, ((float)_amount) / _maxAmount);
            }
        }


        private void Awake()
        {
            Amount = _amount;
        }
        

        private void ClampAmount()
        {
            if (_amount < _minAmount)
                _amount = _minAmount;
            else if (_amount > _maxAmount)
                _amount = _maxAmount;
        }
    }
}
