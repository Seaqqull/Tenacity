using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tenacity
{
    public abstract class DragAndDropController : MonoBehaviour
    {
        //[SerializeField] private LayerMask layerOfObjectToDrag;
        //[SerializeField] private LayerMask layerOfObjectToDrop;

        [SerializeField] private float selectedObjectYPos = 7f;

        protected RaycastHit GetHitWithRaycast(LayerMask layerMask, float distance)
        {
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, distance, layerMask);
            return hit;
        }

        protected GameObject DetectObjectHitWithRaycast(LayerMask layer, float distance)
        {
            RaycastHit hit = GetHitWithRaycast(layer, distance);
            if (hit.collider == null) return null;
            return hit.collider.gameObject;
        }

        protected void DragSelectedObject(GameObject selectedGO)
        {
            Vector3 pos = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedGO.transform.position).z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
            selectedGO.transform.position = new Vector3(worldPos.x, selectedObjectYPos, worldPos.z);
        }

    }
}