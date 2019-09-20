using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMDay
{
	public float m_TimeOfDay = 0f;
}

public class FMGameLoopManager : MonoBehaviourSingleton<FMGameLoopManager>
{
	public FMDay m_CurrentDay = new FMDay();
	public List<FMTaskBase> m_TickableTasks;

	public delegate void OnDayEndEvent(FMDay day);
	public OnDayEndEvent m_OnDayEndEvent;
	public delegate void OnDayStartEvent(FMDay day);
	public OnDayStartEvent m_OnDayStartEvent;

	public float m_DaylightTime = 60f;

	private bool m_IsNightTime = false;

	// Update is called once per frame
	void Update()
    {
		if (m_IsNightTime)
			return;

        for (int i = 0; i < m_TickableTasks.Count; ++i)
		{
			var task = m_TickableTasks[i];
			if (task.m_TaskProcessing)
				m_TickableTasks[i].TickTask(Time.deltaTime);
		}

		m_CurrentDay.m_TimeOfDay += Time.deltaTime;
		if (m_CurrentDay.m_TimeOfDay >= m_DaylightTime)
			StartCoroutine(EndCurrentDay());
	}


	private IEnumerator EndCurrentDay()
	{
		m_IsNightTime = true;
		m_OnDayEndEvent?.Invoke(m_CurrentDay);
		// time for fish to go eat... could be a callback to the game loop manager in future...
		yield return new WaitForSeconds(5f);
		m_CurrentDay = new FMDay();
		m_OnDayStartEvent?.Invoke(m_CurrentDay);
		yield return new WaitForSeconds(5f);
		m_IsNightTime = false;
	}
}
