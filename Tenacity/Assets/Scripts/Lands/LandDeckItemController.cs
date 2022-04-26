using UnityEngine;
using UnityEngine.EventSystems;


namespace Tenacity.Lands
{
    public class LandDeckItemController : MonoBehaviour, IPointerDownHandler
    {

        private LandDeckPlacingController _landDeck;


        private void Awake()
        {
            if (GetComponentInParent<LandDeckPlacingController>())
            {
                _landDeck = GetComponentInParent<LandDeckPlacingController>();
            }
        }


        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if (!_landDeck.enabled) return;
            if (_landDeck.IsCurrentlyPlacingLand) return;

            _landDeck.SelectLand(gameObject.GetComponent<Land>());
        }

    }
}