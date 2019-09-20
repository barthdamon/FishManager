using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMWorker : MonoBehaviour
{
	public float m_SicknessSleepInScalar = 1f;

	private float m_SicknessLevel = 0f;
	private float m_TimeToReturnToWork = 0f;

	private float m_TimeSleptIn = 0f;
	public bool m_IsSleepingIn = false;

	private void Start()
	{
		FMGameLoopManager.GetOrCreateInstance().m_OnDayEndEvent += OnDayEnd;
	}

	public void OnDayEnd(FMDay currentDay)
	{
		GetSomeGrub();
	}

	private void Update()
	{
		if (m_IsSleepingIn && !FMGameLoopManager.GetOrCreateInstance().m_CurrentDay.m_IsNightTime)
		{
			TickReturnToWork(Time.deltaTime);
		}
	}

	public void GoHome()
	{
		// animate go home
		m_TimeSleptIn = 0f;
		m_IsSleepingIn = true;
	}

	public void GetSomeGrub()
	{
		// animate get food
		int randomSink = Random.Range(0, 3);
		var sink = FMBoardReferences.GetOrCreateInstance().m_ResourceSinks[randomSink];
		m_SicknessLevel = sink.m_CurrentSicknessLevel;
		m_TimeToReturnToWork = m_SicknessSleepInScalar *= m_SicknessLevel;
		GoHome();
	}

	public void GoToWorkerPool()
	{
		m_IsSleepingIn = false;
		// move the sprite to pool for selection...
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
