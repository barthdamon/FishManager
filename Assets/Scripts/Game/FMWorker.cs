using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMWorker : MonoBehaviour
{
	public float m_SicknessSleepInScalar = 1f;

	public float m_SicknessLevel = 0f;
	private float m_TimeToReturnToWork = 0f;

	private float m_TimeSleptIn = 0f;
	public bool m_IsSleepingIn = false;

	public FMTaskBase currentTask;    //a worker can only have one job at a time, or none
	
	public void Assign(FMTaskBase task)
	{
        //SoundManager.GetOrCreateInstance().play_audio("fish_drop");

        Debug.Log("Assign Job " + this.name, this);
		Tutorial.GetOrCreateInstance().HasDraggedWorker = true;
		Tutorial.GetOrCreateInstance().HasAssignedWorkerToBoat = true;

		if (currentTask != null)
		{
            SoundManager.GetOrCreateInstance().play_audio("fish_remove");
            currentTask.RemoveWorker(this);
			currentTask = null;
		}

		currentTask = task;
        if (currentTask != null)
        {
            SoundManager.GetOrCreateInstance().play_audio("fish_assign");
            currentTask.AssignWorker(this);
        }
        else
        {
            SoundManager.GetOrCreateInstance().play_audio("fish_drop");
        }
    }

	private void Awake()
	{
		var draggable = GetComponent<FMDraggable>();
		draggable.m_DragReaction += ReactToSelected;
		draggable.m_OnAssignedTask += Assign;
	}

	private void Start()
	{
		FMGameLoopManager.GetOrCreateInstance().m_CurrentDay.m_OnEveningStartEvent += OnEveningStart;
		FMGameLoopManager.GetOrCreateInstance().m_CurrentDay.m_OnDayEndEvent += OnDayEnd;
	}

	public void OnEveningStart(FMDay currentDay)
	{
		GetSomeGrub();
	}

	public void OnDayEnd(FMDay currentDay)
	{
		GoHome();
	}

	public bool ReactToSelected()
	{
        SoundManager.GetOrCreateInstance().play_audio("fish_grab");
		Tutorial.GetOrCreateInstance().HasDraggedWorker = true;
		// TODO: Be pissed if sleeping
		return !m_IsSleepingIn;
	}

	private void Update()
	{
		var time_of_day = FMGameLoopManager.GetOrCreateInstance().m_CurrentDay.GetTimeOfDay();
		if (m_IsSleepingIn && time_of_day != FMDay.TimeOfDay.Evening && time_of_day != FMDay.TimeOfDay.Night)
		{
			TickReturnToWork(Time.deltaTime);
		}
	}

	public void GoHome()
	{
		// animate go home
		m_TimeSleptIn = 0f;
		m_IsSleepingIn = true;

		var drag_handler = gameObject.GetComponent<FMDraggable>();
		if (drag_handler)
			drag_handler.enabled = false;
		
		currentTask = null;

		FMBoardReferences.GetOrCreateInstance().m_WorkerHomeStagingArea.AddToStaging(this.transform);
	}

	public void GetSomeGrub()
	{
		// animate get food
		int randomSink = Random.Range(0, 3);
		var sink = FMBoardReferences.GetOrCreateInstance().m_ResourceSinks[randomSink];
		m_SicknessLevel = sink.m_CurrentSicknessLevel;
		m_TimeToReturnToWork = m_SicknessSleepInScalar * m_SicknessLevel;

		var drag_handler = gameObject.GetComponent<FMDraggable>();
		if (drag_handler)
			drag_handler.enabled = false;

		currentTask = null;

		sink.m_WorkerStagingArea.AddToStaging(this.transform);
		//GoHome();
	}

	public void GoToWorkerPool()
	{
		var drag_handler = gameObject.GetComponent<FMDraggable>();
		if (drag_handler)
			drag_handler.enabled = true;

		currentTask = null;

		m_IsSleepingIn = false;
		// move the sprite to pool for selection...
		FMBoardReferences.GetOrCreateInstance().m_WorkerPoolStagingArea.AddToStaging(this.transform);
	}

	public void TickReturnToWork(float time)
	{
		if (m_IsSleepingIn)
		{
			m_TimeSleptIn += time;
			if (m_TimeSleptIn >= m_TimeToReturnToWork)
				GoToWorkerPool();
		}
	}

	public float GetProductivity()
	{
		return 1 - m_SicknessLevel;
	}
}
