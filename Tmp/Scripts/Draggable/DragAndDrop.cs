using UnityEngine;
using EngineInput = UnityEngine.Input;

namespace Tenacity.Draggable
{
    public abstract class DragAndDrop<T> : MonoBehaviour
    {
        [SerializeField] private LayerMask draggableObjectLayer;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float droppedObjectYPos = 0.61f;
        [SerializeField] private float selectedObjectRescaleDt = 1.1f;
        [SerializeField] private float rayDetectionDistance = 1000.0f;

        private GameObject _selectedGO;
        private Vector3 _previousSelectedObjectPos;

        protected GameObject SelectedGO => _selectedGO;
        protected float DroppedObjectYPos => droppedObjectYPos;


        private void Update()
        {
            if (EngineInput.GetMouseButtonDown(0)) StartDraggingObject();
            if (_selectedGO == null) return;
            if (EngineInput.GetMouseButton(0)) DragSelectedObject(_selectedGO);
            if (EngineInput.GetMouseButtonUp(0)) DropSelectedObject();
        }

        private void DropSelectedObject()
        {   
            GameObject target = DetectObjectHitWithRaycast(targetLayer, rayDetectionDistance);
            if (target == null) GetBackSelectedObject();
            else
            {
                DropSelectedObject(target);
                SelectObjectOnClick(null);
            }
        }

        private void DragSelectedObject(GameObject selectedGO)
        {
            Vector3 pos = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedGO.transform.position).z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
            selectedGO.transform.position = new Vector3(worldPos.x, droppedObjectYPos, worldPos.z);
        }

        private void SelectObjectOnClick(GameObject clickedObject)
        {
            if (clickedObject != null) clickedObject.transform.localScale *= selectedObjectRescaleDt;
            if (_selectedGO != null) _selectedGO.transform.localScale /= selectedObjectRescaleDt;
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

        protected virtual bool StartDraggingObject()
        {
            GameObject clickedObject = DetectObjectHitWithRaycast(draggableObjectLayer, rayDetectionDistance);

            if ((clickedObject != null) && (_selectedGO != clickedObject) && IsDraggable(clickedObject))
            {
                OnStartDragging(clickedObject);
                return true;
            }

            if (_selectedGO != null) GetBackSelectedObject();
            return false;
        }

        protected void OnStartDragging(GameObject clickedObject)
        {
            SelectObjectOnClick(clickedObject);
            _previousSelectedObjectPos = clickedObject.transform.position;
        }
        
        protected GameObject DetectObjectHitWithRaycast()
        {
            return DetectObjectHitWithRaycast(draggableObjectLayer, rayDetectionDistance);
        }

        protected void PlaceSelectedObject(GameObject target)
        {
            _selectedGO.transform.parent = target.transform;
            _selectedGO.transform.localPosition = new Vector3(0, DroppedObjectYPos, 0);
        }

        protected abstract bool DropSelectedObject(GameObject target);
        protected abstract bool IsDraggable(GameObject gameObject);
    }
}