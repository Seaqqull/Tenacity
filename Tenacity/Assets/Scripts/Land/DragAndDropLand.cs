using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tenacity.Lands
{
    public class DragAndDropLand : DragAndDropController
    {
        [SerializeField] private LayerMask landLayer;
        [SerializeField] private LayerMask cardLandLayer;

        [SerializeField] [Range(0.1f, 1.5f)] private float selectedLandScale = 0.6f;

        private string DroppedLandAssetPath
        {
            get => ( _selectedLand != null)
                    ? $"Assets/StaticAssets/Prefabs/Lands/land_{_selectedLand.Type}.prefab"
                    : "";
        }

        private Land _selectedLand;
        private Vector3 _previousSeletedLandPos;

        private readonly float rayLandDetectionDistance = 1000.0f;

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                TryTakeLandToDrag();
            }
            if (_selectedLand == null) return;

            if (UnityEngine.Input.GetMouseButton(0))
            {
                DragSelectedObject(_selectedLand.gameObject);
            }
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                TryDropSelectedLand();
            }
        }

        private void SelectLandOnClick(Land clickedLand)
        {
            if (clickedLand != null) clickedLand.transform.localScale *= selectedLandScale;
            if (_selectedLand != null) _selectedLand.transform.localScale /= selectedLandScale;
            _selectedLand = clickedLand;
        }

        private void TryDropSelectedLand()
        {
            GameObject detectedLanddGO = DetectObjectHitWithRaycast(landLayer, rayLandDetectionDistance);
            if (detectedLanddGO == null)
            {
                DismissSelectedLand();
                return;
            }
            Land detectedLand = detectedLanddGO.GetComponent<Land>();

            if (detectedLand == null || !detectedLand.IsPlacedOnBoard || detectedLand.Type != LandType.None)
            {
                DismissSelectedLand();
            }
            else
            {
                CreateLandOnBord(detectedLand);
            }
        }

        private void DismissSelectedLand()
        {
            _selectedLand.transform.position = _previousSeletedLandPos;
            SelectLandOnClick(null);
        }

        private void TryTakeLandToDrag()
        {
            GameObject clickedCardLandGO = DetectObjectHitWithRaycast(cardLandLayer, rayLandDetectionDistance);
            if (clickedCardLandGO == null) return;

            Land clickedCardLand = clickedCardLandGO.GetComponent<Land>();

            if (clickedCardLand == null || clickedCardLand.IsPlacedOnBoard) return;

            if (_selectedLand != clickedCardLand)
            {
                SelectLandOnClick(clickedCardLand);
                _previousSeletedLandPos = clickedCardLand.transform.position;
            }
            else
            {
                if (_selectedLand == null) return;
                DismissSelectedLand();
            }
        }

        private void CreateLandOnBord(Land landToPlace)
        {
            Object cardCreaturePref = AssetDatabase.LoadAssetAtPath(DroppedLandAssetPath, typeof(GameObject));
            if (cardCreaturePref == null) return;

            GameObject placedLandGO = Instantiate(cardCreaturePref, landToPlace.transform.position, Quaternion.identity) as GameObject;
            placedLandGO.transform.parent = landToPlace.transform.parent;
            
            Land placedLand = placedLandGO.GetComponent<Land>();
            placedLand.IsPlacedOnBoard = true;
            placedLand.IsAvailable = true;

            Destroy(landToPlace.gameObject);
            Destroy(_selectedLand.gameObject);
        }
    }
}