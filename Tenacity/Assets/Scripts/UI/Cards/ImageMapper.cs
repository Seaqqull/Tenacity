using UnityEngine.UI;
using UnityEngine;


namespace Tenacity.UI
{
    public class ImageMapper : MonoBehaviour
    {
        [System.Serializable]
        public class ImageMap
        {
            public int Value;
            public Sprite Source;
        }


        [SerializeField] private int _value;
        [Space]
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _defaultSource;
        [SerializeField] private ImageMap[] _mappedImages;
        
        public int Value
        {
            set
            {
                if (value == _value) return;
                
                foreach (var mappedImage in _mappedImages)
                {
                    if (value == mappedImage.Value)
                    {
                        _image.sprite = mappedImage.Source;
                        return;
                    }
                }

                _value = -1;
                _image.sprite = _defaultSource;
            }
        }


        private void Awake()
        {
            Value = _value;
        }
    }
}
