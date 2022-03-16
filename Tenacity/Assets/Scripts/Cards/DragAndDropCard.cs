using System;
using Tenacity.Lands;
using UnityEditor;
using UnityEngine;

namespace Tenacity.Cards
{
    public class DragAndDropCard : DragAndDropController
    {
        [SerializeField] private LayerMask cardLayer;
        [SerializeField] private LayerMask landLayer;
        [SerializeField] [Range(0.1f, 1.5f)] private float selectedCardScale = 1.1f;

        private string DroppedCardAssetPath
        {
            get =>  (_selectedCard != null && _selectedCard.Data != null)
                    ? $"Assets/StaticAssets/Prefabs/Creatures/{_selectedCard.Data.Type}_{_selectedCard.Data.CardId}.prefab"
                    : "";
        }

        private Card _selectedCard;
        private Vector3 _previousSeletedCardPos;

        private readonly float droppedCardYPos = 0.61f;
        private readonly float rayLandDetectionDistance = 1000.0f;

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                TryTakeCardToDrag();
            }
            if (_selectedCard == null) return;

            if (UnityEngine.Input.GetMouseButton(0))
            {
                DragSelectedObject(_selectedCard.gameObject);
            }
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                TryDropSelectedCard();
            }
        }

        private void SelectCardOnClick(Card clickedCard)
        {
            if (clickedCard != null) clickedCard.transform.localScale *= selectedCardScale;
            if (_selectedCard != null) _selectedCard.transform.localScale /= selectedCardScale;
            _selectedCard = clickedCard;
        }

        private void TryDropSelectedCard()
        {
            Land land = DetectLandHitWithRaycast();

            if (land != null && land.IsAvailable && land.Type == _selectedCard.Data.Land)
            {
                DropSelectedCardIntoLand(land);
            }
            else
            {
                _selectedCard.transform.position = _previousSeletedCardPos;
            }
            SelectCardOnClick(null);
        }

        private void CreateCardOnBoard(Land land)
        {
            UnityEngine.Object cardCreaturePref = AssetDatabase.LoadAssetAtPath(DroppedCardAssetPath, typeof(GameObject));
            if (cardCreaturePref == null) return;

            GameObject cardCreatureGO = Instantiate(cardCreaturePref) as GameObject;
            cardCreatureGO.transform.parent = land.transform;
            cardCreatureGO.transform.localPosition = new Vector3(0, droppedCardYPos, 0);
            if (cardCreatureGO.GetComponent<Card>() != null)
            {
                Card droppedCard = cardCreatureGO.GetComponent<Card>();
                droppedCard.Data = _selectedCard.Data;
                droppedCard.State = CardState.OnBoard;
            }
            Destroy(_selectedCard.gameObject);
        }

        private void DropSelectedCardIntoLand(Land land)
        {

            if (_selectedCard.transform.parent.TryGetComponent<Land>(out Land currentLand))
            {
                currentLand.IsAvailable = true;
            }
            land.IsAvailable = false;

            if (_selectedCard.State != CardState.OnBoard)
            {
                CreateCardOnBoard(land);
            }
            else
            {
                _selectedCard.transform.parent = land.transform;
                _selectedCard.transform.localPosition = new Vector3(0, droppedCardYPos, 0);
            }

        }

        private void TryTakeCardToDrag()
        {
            Card clickedCard = DetectCardHitWithRaycast();

            if ((clickedCard != null) && (_selectedCard != clickedCard))
            {
                _previousSeletedCardPos = clickedCard.transform.position;
                SelectCardOnClick(clickedCard);
            }
            else
            {
                if (_selectedCard == null) return;
                _selectedCard.transform.position =_previousSeletedCardPos;
                SelectCardOnClick(null);
            }
        }

        private Land DetectLandHitWithRaycast()
        {
            GameObject detectedLandGO = DetectObjectHitWithRaycast(landLayer, rayLandDetectionDistance);
            if (detectedLandGO == null || detectedLandGO.GetComponent<Land>() == null) return null;

            return detectedLandGO.GetComponent<Land>();
        }
        private Card DetectCardHitWithRaycast()
        {
            GameObject detectedCardGO = DetectObjectHitWithRaycast(cardLayer, rayLandDetectionDistance);
            if (detectedCardGO == null || detectedCardGO.GetComponent<Card>() == null) return null;

            return detectedCardGO.GetComponent<Card>();
        }

    }
}