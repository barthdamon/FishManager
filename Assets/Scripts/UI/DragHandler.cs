using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform drag_start_parent = null;
    private Vector3 drag_start_offset = Vector3.zero;
    private Vector3 drag_start_pos = Vector3.zero;
    private Quaternion drag_start_rot = Quaternion.identity;

    public void OnBeginDrag(PointerEventData event_data)
    {
        /// TODO: This determines the offset from the centre of the dragged body. Currently it will
        /// only work for UI components (those with a RectTransform rather than a Transform).
        var rect_transform = this.transform as RectTransform;
        if (rect_transform != null)
        {
            this.drag_start_offset =
                Camera.main.ScreenToWorldPoint(event_data.pressPosition) - rect_transform.position;
        }

        // Save the original transform details before messing with them.
        this.drag_start_pos = this.transform.localPosition;
        this.drag_start_rot = this.transform.localRotation;

        // Unparent the dragged object while dragging to prevent
        // the sort order from causing it to pass behind other
        // images.
        this.drag_start_parent = this.transform.parent;
        var canvas = this.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            this.transform.SetParent(canvas.transform);
        }

        // Prevent the image from being a raycast target, which will
        // prevent actually detecting the drop target.
        var image = this.GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            image.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData event_data)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(event_data.position) - this.drag_start_offset;
        pos.z = 100.0f;
        this.transform.position = pos;
        this.transform.rotation = Quaternion.identity;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore the original transform details.
        this.transform.localPosition = this.drag_start_pos;
        this.transform.localRotation = this.drag_start_rot;

        // Restore the original parent.
        this.transform.SetParent(this.drag_start_parent);

        // Make the image a raycast target again.
        var image = this.GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            image.raycastTarget = true;
        }
    }
}
