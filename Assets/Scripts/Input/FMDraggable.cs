using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FMDraggable : Selectable, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool dragOnSurfaces = true;
    public bool dragCopy = false;

    private GameObject m_DraggingIcon;
    private RectTransform m_DraggingPlane;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("BeginDrag " + this.name, this);
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;

        // We have clicked something that can be dragged.
        // What we want to do is create an icon for this.
        if (dragCopy)
        {
            m_DraggingIcon = new GameObject("icon");

            m_DraggingIcon.transform.SetParent(canvas.transform, false);
            m_DraggingIcon.transform.SetAsLastSibling();

            var image = m_DraggingIcon.AddComponent<Image>();

            image.sprite = GetComponent<Image>().sprite;
            image.SetNativeSize();
        }

        if (dragOnSurfaces)
            m_DraggingPlane = transform as RectTransform;
        else
            m_DraggingPlane = canvas.transform as RectTransform;

        GetComponent<Image>().raycastTarget = false;

        SetDraggedPosition(eventData);

        FMInputController.GetOrCreateInstance().currentlyDragging = this.gameObject;
    }

    public void OnDrag(PointerEventData data)
    {
        //if (m_DraggingIcon != null)
            SetDraggedPosition(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = dragCopy ? m_DraggingIcon.GetComponent<RectTransform>() : this.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag " + this.name, this);

        if (m_DraggingIcon != null)
        {
            this.transform.position = m_DraggingIcon.transform.position;
            Destroy(m_DraggingIcon);
        }

        if (FMInputController.GetOrCreateInstance().currentlyDragging == this.gameObject)
        {
            FMInputController.GetOrCreateInstance().OnDropped(this.gameObject, FMInputController.GetOrCreateInstance().currentlyOver);
            FMInputController.GetOrCreateInstance().currentlyDragging = null;
        }

        GetComponent<Image>().raycastTarget = true;
    }

    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}