using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FMDropTarget : Selectable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData eventData) // 3
    {
        Debug.Log("OnClick "+ this.name, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //print("I'm being dragged!");
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter " + this.name, this);

        base.OnPointerEnter(eventData);

        FMInputController.GetOrCreateInstance().currentlyOver = this;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit " + this.name, this);

        base.OnPointerExit(eventData);

        if (FMInputController.GetOrCreateInstance().currentlyOver == this)
            FMInputController.GetOrCreateInstance().currentlyOver = null;
    }
}
