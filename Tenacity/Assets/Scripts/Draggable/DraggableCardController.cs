using Tenacity.Cards;
using Tenacity.Lands;
using UnityEditor;
using UnityEngine;

namespace Tenacity.Draggable
{
    public class DraggableCardController : DragAndDrop<Card>
    {
        private Card SelectedCard => SelectedGO?.GetComponent<Card>();

        protected override void DropSelectedObject()
        {
            var parentLand = SelectedCard.transform.parent?.GetComponent<LandController>();
            if (parentLand != null) parentLand.HighlightNeighbors(SelectedCard.Data.Land, false);

            base.DropSelectedObject();
        }

        protected override bool DropSelectedObject(GameObject target)
        {
            if (target.GetComponent<Land>() == null) return false;

            Land land = target.GetComponent<Land>();
            var parentLand = SelectedCard.transform.parent?.GetComponent<Land>();

            //if target land is not a 'neighbor'
            if (parentLand != null && !parentLand.NeighborLands.Contains(land))
            {
                GetBackSelectedObject();
                return false;
            }
            //if not available or doesn't match card type
            if (!land.IsAvailableForCards || !land.Type.HasFlag(SelectedCard.Data.Land))
            {
                GetBackSelectedObject();
                return false;
            }

            if (SelectedCard.transform.parent != null
                && SelectedCard.transform.parent.TryGetComponent<Land>(out Land prevLand))
            {
                prevLand.IsAvailableForCards = true;
            }
            land.IsAvailableForCards = false;

            if (SelectedCard.State != CardState.OnBoard)
            {
                SelectedCard.transform.parent = land.transform;
                SelectedCard.transform.localPosition = new Vector3(0, DroppedObjectYPos, 0);
                CardController.CreateCardCreatureOnBoard(SelectedCard);
            }
            else
            {
                PlaceSelectedObject(target);
            }
            return true;
        }

        protected override bool IsDraggable(GameObject gameObject)
        {
            return gameObject.GetComponent<Card>() && gameObject.GetComponent<Card>().IsDraggable;
        }

        protected override void OnStartDragging(GameObject clickedObject)
        {
            base.OnStartDragging(clickedObject);
            var parentLand = clickedObject.transform.parent?.GetComponent<LandController>();
            if (parentLand != null) parentLand.HighlightNeighbors(SelectedCard.Data.Land, true);
        }
    }
}