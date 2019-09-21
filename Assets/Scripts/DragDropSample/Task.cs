using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    [Range(0, 1)]
    public float progress;

    public Image fillerImage;
    public Image baseImage;

    public List<Worker> workers = new List<Worker>();   //a task can have more than one workers at a time

    void OnEnable()
    {
        TaskManager.Get().tasks.Add(this);
    }

    void OnDisable()
    {
        TaskManager.Get().tasks.Remove(this);
    }

    void FixedUpdate()
    {
        //mechanics
        if (workers.Count > 0)
            SetProgress(progress + workers.Count * 0.01f * Time.fixedDeltaTime);
        
        //ToDo: decoration; e.g. pulse when being worked on
    }

    void SetProgress(float f)
    {
        f = Mathf.Clamp01(f);
        if (progress == f) return;
        progress = f;

        if (fillerImage != null)
        {
            if (fillerImage.type == Image.Type.Filled)
            {
                fillerImage.fillAmount = progress;
            }
            else
            {
                //ToDo
            }
        }

        if (progress >= 1.0f)
        {
            Debug.Log("FINISHED! " + this.name);
        }
    }
}
