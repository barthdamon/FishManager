using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMInputController : MonoBehaviourSingleton<FMInputController>
{
    public GameObject currentlyDragging;
    public FMDropTarget currentlyOver;

    public void OnDropped(GameObject draggable, FMDropTarget toTarget)
    {
        //ToDo: check if the draggable is a worker; if not, ignore
        //if it is, 
        // check if it is currently assigned to a task; if it is, UNASSIGN (tell both it and the task)
        // check if what it's dropped into is a task; if it is, ASSIGN (tell both it and the task)
        FMDraggable draggableComponent = draggable.GetComponent<FMDraggable>();
		if (draggableComponent == null)
			return;

		FMTaskBase task = toTarget == null ? null : toTarget.GetComponent<FMTaskBase>();

		// assign to either a null task or a task that can accept a worker
		bool shouldAssign = (task == null);
		shouldAssign |= (task != null && task.AcceptsWorkers());
		if (shouldAssign)
        {
			draggableComponent.Assign(task);
        } else
		{
			draggableComponent.Unassign();
		}
    }
}
