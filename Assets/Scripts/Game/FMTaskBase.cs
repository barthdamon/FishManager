using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FMTaskBase : MonoBehaviour
{
	public bool m_TaskProcessing = true;
	public float m_TimeSinceLastTrigger = 0f;

	public Queue<FMResource> m_Resources = new Queue<FMResource>();
	public List<FMWorker> m_AssignedWorkers = new List<FMWorker>();

	private void Start()
	{
		FMGameLoopManager.GetOrCreateInstance().m_OnDayEndEvent += OnDayEnd;
	}

	public void OnDayEnd(FMDay day)
	{
		foreach (var item in m_AssignedWorkers)
		{
			item.GetSomeGrub();
		}
	}

	// Returns if true if task is now processing
	public virtual bool AssignWorker(FMWorker worker)
	{
		if (!m_AssignedWorkers.Contains(worker))
		{
			m_AssignedWorkers.Add(worker);
			return true;
		}
		return false;
	}

	public virtual bool RemoveWorker(FMWorker worker)
	{
		if (m_AssignedWorkers.Contains(worker))
		{
			m_AssignedWorkers.Remove(worker);
			return true;
		}
		return false;
	}

	protected virtual void TriggerTask()
	{
		m_TimeSinceLastTrigger = 0f;
	}

	public abstract float GetTimeToTrigger();

	public virtual void TickTask(float time)
	{
		m_TimeSinceLastTrigger += time;
		if (GetTimeToTrigger() - m_TimeSinceLastTrigger <= 0)
		{
			TriggerTask();
		}
	}
}
