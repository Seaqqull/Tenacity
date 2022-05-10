using UnityEngine.EventSystems;
using UnityEngine;


namespace Tenacity.Battles.Lands
{
    public class LandDeckItemController : MonoBehaviour, IPointerDownHandler
    {
        private LandDeckPlacingController _landDeck;


        private void Awake()
        {
            if (GetComponentInParent<LandDeckPlacingController>() != null)
                _landDeck = GetComponentInParent<LandDeckPlacingController>();
        }


        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if (!_landDeck.enabled  || _landDeck.IsCurrentlyPlacingLand) return;
            _landDeck.SelectLand(gameObject.GetComponent<Land>());
        }

    }
}