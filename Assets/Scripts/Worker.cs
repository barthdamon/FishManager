using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public Task currentTask;    //a worker can only have one job at a time, or none

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public void Assign(Task task)
    {
        Debug.Log("Assign Job " + this.name, this);

        if (currentTask != null)
        {
            currentTask.workers.Remove(this);
            currentTask = null;
        }

        currentTask = task;
        if (currentTask != null)
            currentTask.workers.Add(this);
    }
}
