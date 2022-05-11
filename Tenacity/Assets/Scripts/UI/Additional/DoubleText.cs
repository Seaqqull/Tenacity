using UnityEngine;
using TMPro;


namespace Tenacity.UI.Additional
{
    public class DoubleText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _mainText;
        [SerializeField] private TMP_Text _additionalText;

        public string Text
        {
            set
            {
                _mainText.text = value;
                _additionalText.text = value;
            }
        }
    }
}
