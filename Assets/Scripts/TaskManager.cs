using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager singleton;

    public GameObject currentlyDragging;
    public DropTarget currentlyOver;

    public List<Task> tasks = new List<Task>();

    private void Awake()
    {
        singleton = this;
    }

    public static TaskManager Get()
    {
        if (singleton == null)
        {
            GameObject go = new GameObject("TaskManager");
            singleton = go.AddComponent<TaskManager>();
        }
        return singleton;
    }

    public void OnDropped(GameObject draggable, DropTarget toTarget)
    {
        //ToDo: check if the draggable is a worker; if not, ignore
        //if it is, 
        // check if it is currently assigned to a task; if it is, UNASSIGN (tell both it and the task)
        // check if what it's dropped into is a task; if it is, ASSIGN (tell both it and the task)
        Worker worker = draggable.GetComponent<Worker>();
        Task task = toTarget==null ? null : toTarget.GetComponent<Task>();
        if (worker != null)
        {
            worker.Assign(task);
        }
    }
}
