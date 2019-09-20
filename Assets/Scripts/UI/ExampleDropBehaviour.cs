using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleDropBehaviour : MonoBehaviour
{
    [SerializeField]
    private bool accepts_drops = true;

    public bool CanDrop(GameObject dragged_object)
    {
        return this.accepts_drops;
    }

    public void SetHighlight(bool highlight)
    {
    }

    public void Dropped(GameObject dropped_object)
    {
    }
}
