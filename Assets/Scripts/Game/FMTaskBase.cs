using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class FMTaskBase : MonoBehaviour
{
	[Range(0, 1)]
	public float progress;

	public Image fillerImage;
	public Image baseImage;

	public bool m_TaskProcessing = true;
	public float m_TimeSinceLastTrigger = 0f;

	public Queue<FMResource> m_Resources = new Queue<FMResource>();
	public List<FMWorker> m_AssignedWorkers = new List<FMWorker>();

	public virtual void SetProgress(float f)
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

	public virtual bool AcceptsWorkers()
	{
		return true;
	}

	private void OnEnable()
	{
		FMGameLoopManager.GetOrCreateInstance().m_TickableTasks.Add(this);
	}

	private void OnDisable()
	{
		FMGameLoopManager.GetOrCreateInstance().m_TickableTasks.Remove(this);
	}

	private void Start()
	{
		FMGameLoopManager.GetOrCreateInstance().m_OnDayEndEvent += OnDayEnd;
	}

	public void OnDayEnd(FMDay day)
	{
		m_AssignedWorkers.Clear();
		ShutDown();
		// might need animations to stop functioning
	}

	protected virtual void ShutDown()
	{

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
		UpdateDisplayProgress();
		if (GetTimeToTrigger() - m_TimeSinceLastTrigger <= 0)
		{
			TriggerTask();
		}
	}

	protected virtual void UpdateDisplayProgress()
	{
		SetProgress(m_TimeSinceLastTrigger / GetTimeToTrigger());
	}
}
