using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FMWorkerPool : MonoBehaviour
{
	private List<FMWorker> m_AllWorkers = new List<FMWorker>();

	public IEnumerable<FMWorker> GetAvailableWorkers()
	{
		return m_AllWorkers.Where((FMWorker worker) => !worker.m_IsSleepingIn);
	}

	private IEnumerable<FMWorker> GetSleepingWorkers()
	{
		return m_AllWorkers.Where((FMWorker worker) => worker.m_IsSleepingIn);
	}

	private void Start()
	{
		FMGameLoopManager.GetOrCreateInstance().m_OnDayStartEvent += OnDayStart;
		FMGameLoopManager.GetOrCreateInstance().m_OnDayEndEvent += OnDayEnd;
	}

	public void OnDayStart(FMDay currentDay)
	{
		// open up the worker pool game object for business
	}

	public void OnDayEnd(FMDay currentDay)
	{
		// shut down the worker pool
	}
}
