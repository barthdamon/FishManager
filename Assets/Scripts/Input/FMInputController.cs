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
        FMWorker worker = draggable.GetComponent<FMWorker>();
		if (worker == null)
			return;

		if (worker.m_IsSleepingIn)
			return;

		FMTaskBase task = toTarget == null ? null : toTarget.GetComponent<FMTaskBase>();

		// assign to either a null task or a task that can accept a worker
		bool shouldAssignWorker = task == null;
		shouldAssignWorker |= (task != null && task.AcceptsWorkers());
		if (shouldAssignWorker)
        {
            worker.Assign(task);
        }
    }
}
