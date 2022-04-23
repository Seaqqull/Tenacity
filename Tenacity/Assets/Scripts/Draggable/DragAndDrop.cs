using UnityEngine;
using EngineInput = UnityEngine.Input;

namespace Tenacity.Draggable
{
    public abstract class DragAndDrop<T> : MonoBehaviour
    {
        [SerializeField] private LayerMask draggableObjectLayer;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private float _droppedObjectYPos = 0.61f;
        [SerializeField] private float _selectedObjectRescaleDt = 1.1f;
        [SerializeField] private float _rayDetectionDistance = 1000.0f;

        private GameObject _selectedGO;
        private Vector3 _previousSelectedObjectPos;

        protected GameObject SelectedGO => _selectedGO;
        protected float DroppedObjectYPos => _droppedObjectYPos;


        private void Update()
        {
            if (EngineInput.GetMouseButtonDown(0)) StartDraggingObject();
            if (_selectedGO == null) return;
            if (EngineInput.GetMouseButton(0)) MoveWithMouseCursor(_selectedGO);
            if (EngineInput.GetMouseButtonUp(0)) OnEndDragging();
        }

        private void SelectObjectOnClick(GameObject clickedObject)
        {
            if (clickedObject != null) clickedObject.transform.localScale *= _selectedObjectRescaleDt;
            if (_selectedGO != null) _selectedGO.transform.localScale /= _selectedObjectRescaleDt;
            _selectedGO = clickedObject;
        }
        private RaycastHit GetHitWithRaycast(LayerMask layerMask, float distance)
        {
            Ray ray = Camera.main.ScreenPointToRay(EngineInput.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, distance, layerMask);
            return hit;
        }
        private GameObject DetectObjectHitWithRaycast(LayerMask layer, float distance)
        {
            RaycastHit hit = GetHitWithRaycast(layer, distance);
            if (hit.collider == null) return null;
            return hit.collider.gameObject;
        }


        protected void GetBackSelectedObject()
        {
            _selectedGO.transform.position = _previousSelectedObjectPos;
            SelectObjectOnClick(null);
        }
        protected GameObject DetectObjectHitWithRaycast()
        {
            return DetectObjectHitWithRaycast(draggableObjectLayer, _rayDetectionDistance);
        }

        protected virtual bool StartDraggingObject()
        {
            GameObject clickedObject = DetectObjectHitWithRaycast(draggableObjectLayer, _rayDetectionDistance);

            if ((clickedObject != null) && (_selectedGO != clickedObject) && IsDraggable(clickedObject))
            {
                OnStartDragging(clickedObject);
                return true;
            }

            if (_selectedGO != null) GetBackSelectedObject();
            return false;
        }
        protected virtual void MoveWithMouseCursor(GameObject selectedGO)
        {
            Vector3 pos = new Vector3(EngineInput.mousePosition.x, EngineInput.mousePosition.y, Camera.main.WorldToScreenPoint(selectedGO.transform.position).z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
            selectedGO.transform.position = new Vector3(worldPos.x, _droppedObjectYPos, worldPos.z);
        }
        protected virtual void OnStartDragging(GameObject clickedObject)
        {
            SelectObjectOnClick(clickedObject);
            _previousSelectedObjectPos = clickedObject.transform.position;
        }


        protected void PlaceSelectedObject(GameObject target)
        {
            _selectedGO.transform.parent = target.transform;
            _selectedGO.transform.localPosition = new Vector3(0, DroppedObjectYPos, 0);
        }
        protected virtual void OnEndDragging()
        {
            GameObject target = DetectObjectHitWithRaycast(_targetLayer, _rayDetectionDistance);
            if (target == null) GetBackSelectedObject();
            else
            {
                DropSelectedObject(target);
                SelectObjectOnClick(null);
            }
        }
        protected abstract bool DropSelectedObject(GameObject target);

        protected abstract bool IsDraggable(GameObject gameObject);
    
    }
}