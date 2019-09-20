using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private ExampleDropBehaviour ui_script = null;

    [SerializeField]
    private UnityEngine.UI.Image highlight_image = null;

    [SerializeField]
    private Color highlight_good_colour = Color.green;

    [SerializeField]
    private Color highlight_bad_colour = Color.red;

    private bool can_drop = false;
    private bool highlighted = false;
    private float highlight_pulse_lerp = 0.0f;

    public void OnPointerEnter(PointerEventData event_data)
    {
        this.highlighted = true;
        this.can_drop = DropEnabled(event_data.pointerDrag);
        if (!this.can_drop)
            return;

        this.ui_script.SetHighlight(true);
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        this.highlighted = false;
        this.can_drop = DropEnabled(event_data.pointerDrag);
        if (!this.can_drop)
            return;

        this.ui_script.SetHighlight(false);
    }

    public void OnDrop(PointerEventData event_data)
    {
        this.highlighted = false;
        this.can_drop = DropEnabled(event_data.pointerDrag);
        if (!this.can_drop)
            return;

        this.ui_script.Dropped(event_data.pointerDrag);
        this.ui_script.SetHighlight(false);
    }

    private bool DropEnabled(GameObject drag_object)
    {
        return this.ui_script != null && this.ui_script.CanDrop(drag_object);
    }

    protected void Start()
    {
        // Start the highlight pulse animation...
        StartCoroutine(this.PulseHighlight());
    }

    protected void Update()
    {
        if (this.highlight_image != null)
        {
            if (this.highlighted)
            {
                Color highlight_colour = this.can_drop ? this.highlight_good_colour : this.highlight_bad_colour;
                this.highlight_image.color = Color.Lerp(
                    highlight_colour.Adjusted(a: 0.2f),
                    highlight_colour.Adjusted(a: 0.4f),
                    this.highlight_pulse_lerp);
            }
            else
            {
                this.highlight_image.color = this.highlight_image.color.Adjusted(a: 0);
            }
        }
    }

    private IEnumerator PulseHighlight()
    {
        bool pulse_rising = true;
        while (true)
        {
            this.highlight_pulse_lerp += pulse_rising ? Time.deltaTime : -Time.deltaTime;
            if (this.highlight_pulse_lerp > 1.0f)
            {
                pulse_rising = false;
                this.highlight_pulse_lerp = 1.0f;
            }
            else if (this.highlight_pulse_lerp < 0.0f)
            {
                pulse_rising = true;
                this.highlight_pulse_lerp = 0.0f;
            }

            yield return null;
        }
    }
}
