using Tenacity.Battles.Lands.Data;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;
using Tenacity.Battles.Views.Players;


namespace Tenacity.Battles.Views.Land
{
    [RequireComponent(typeof(Outline))]
    public class SingleLandView : PlayerView, IPointerClickHandler
    {
        [SerializeField] private LandType _land;
        
        private Action<LandType> _onClick;
        private Outline _outline;

        public event Action<LandType> OnClick
        {
            add => _onClick += value;
            remove => _onClick -= value;
        }
        public LandType Land
        {
            get => _land;
        }
        public bool Selected
        {
            get => _outline.enabled;
            set
            {
                _outline.enabled = value;
            }
        }

        
        private void Awake()
        {
            _outline = GetComponent<Outline>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke(_land);
        }
    }
}