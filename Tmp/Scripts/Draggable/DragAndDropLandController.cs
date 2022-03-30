using Tenacity.Lands;
using UnityEngine;

namespace Tenacity.Draggable
{
    public class DragAndDropLandController : DragAndDrop<Land>
    {
        private Land SelectedLand => (SelectedGO != null) ? SelectedGO.GetComponent<Land>() : null;

        protected override bool StartDraggingObject()
        {
            GameObject clickedObject = DetectObjectHitWithRaycast();
            if (clickedObject == null) return false;

            Land clickedCardLand = clickedObject.GetComponent<Land>();
            if (clickedCardLand == null || clickedCardLand.IsPlacedOnBoard) return false;

            if (SelectedLand != clickedCardLand) OnStartDragging(clickedObject);
            else if (SelectedLand != null) GetBackSelectedObject();
            return true;
        }

        protected override bool DropSelectedObject(GameObject target)
        {
            if (target.GetComponent<Land>() == null) return false;
            Land detectedLand = target.GetComponent<Land>();

            if (detectedLand == null || !detectedLand.IsPlacedOnBoard || detectedLand.Type != LandType.None)
            {
                GetBackSelectedObject();
                return false;
            }
            else
            {
                detectedLand.ReplaceEmptyLand(SelectedLand);
                return true;
            }
        }

        protected override bool IsDraggable(GameObject gameObject)
        {
            return true;
        }

    }
}