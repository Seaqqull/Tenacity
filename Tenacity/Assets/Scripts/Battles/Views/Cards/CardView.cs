using Tenacity.Battles.Lands.Data;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Tenacity.Cards;
using Tenacity.Base;
using UnityEngine;
using System;
using System.Linq;
using TMPro;


namespace Tenacity.Battles.Views.Cards
{
    public class CardView : BaseMono, IPointerClickHandler
    {
        [System.Serializable]
        private class LandImage
        {
            public LandType Land;
            public Sprite Image;
        }
        [SerializeField] private Vector3 _selectionShift;
        [Space]
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _manaCost;
        [SerializeField] private TMP_Text _landCost;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _attack;
        [Space] 
        [SerializeField] private Image _image;
        [SerializeField] private Image _landImage;
        [Header("Ground types")] 
        [SerializeField] private LandImage[] _landImages;

        private Vector3 _startPosition;
        private Action _onClick;
        private bool _selected;

        public event Action OnClick
        {
            add => _onClick += value;
            remove => _onClick -= value;
        }
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                Transform.localPosition = _selected ? (_startPosition + _selectionShift) : _startPosition;
            }
        }


        protected override void Awake()
        {
            base.Awake();

            _startPosition = Transform.localPosition;
        }
        
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke();
        }
        
        
        public void FillData(CardSO cardData)
        {
            _manaCost.text = cardData.CastingCost.ToString();
            _landCost.text = cardData.LandCost.ToString();
            _attack.text = cardData.Power.ToString();
            _health.text = cardData.Life.ToString();
            _image.sprite = cardData.InventoryView;
            _description.text = cardData.CardText;
            _name.text = cardData.Name;
            
            var enableLandInfo = (cardData.LandCost != 0);
            _landImage.gameObject.SetActive(enableLandInfo);
            _landCost.gameObject.SetActive(enableLandInfo);

            var landData = _landImages.FirstOrDefault(landData => landData.Land == cardData.Land);
            if (landData != null)
                _landImage.sprite = landData.Image;
        }
    }
}