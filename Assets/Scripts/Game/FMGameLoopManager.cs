using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FMDay
{
	public enum TimeOfDay
	{
		Morning,
		Day,
		Evening,
		Night,
	}

	//public TimeOfDay m_TimeOfDay = TimeOfDay.Morning;
	//public float m_HourOfDay = 0f;

	public float m_MorningTime = 5f;
	public float m_DaylightTime = 60f;
	public float m_EveningTime = 5f;
	public float m_NightTime = 10f;

	public delegate void OnTimeOfDayChangedEvent(FMDay day);

	public OnTimeOfDayChangedEvent m_OnMorningStartEvent;
	public OnTimeOfDayChangedEvent m_OnDayStartEvent;
	public OnTimeOfDayChangedEvent m_OnEveningStartEvent;
	public OnTimeOfDayChangedEvent m_OnDayEndEvent;

	private TimeOfDay m_last_time_of_day = TimeOfDay.Night;
    public int m_CurrentDay = 1;
    public float m_HourOfDay = 5f;

	public float GetNormalisedTimeOfDay()
	{
		float hours_in_the_day = m_MorningTime + m_DaylightTime;
		float daylight_hour = Mathf.Min(m_HourOfDay, hours_in_the_day);
		return daylight_hour / hours_in_the_day;
	}

	public float GetNormalisedTimeOfNight()
	{
		float hours_in_the_day = m_MorningTime + m_DaylightTime;
		float hours_in_the_night = m_EveningTime + m_NightTime;
		float night_hour = Mathf.Min(m_HourOfDay - hours_in_the_day, hours_in_the_night);
		return night_hour / hours_in_the_night;
	}

	public void IncrementTime(float delta_time)
	{
		float length_of_day = m_MorningTime + m_DaylightTime + m_EveningTime + m_NightTime;
		m_HourOfDay += delta_time;
		if (m_HourOfDay >= length_of_day)
		{
			m_HourOfDay -= length_of_day;
            m_CurrentDay++;

        }

		TimeOfDay current_time_of_day = GetTimeOfDay();
		if (m_last_time_of_day != current_time_of_day)
		{
			switch (current_time_of_day)
			{
				case TimeOfDay.Morning:
					m_OnMorningStartEvent?.Invoke(this);
					break;
				case TimeOfDay.Day:
					m_OnDayStartEvent?.Invoke(this);
					break;
				case TimeOfDay.Evening:
					m_OnEveningStartEvent?.Invoke(this);
					break;
				case TimeOfDay.Night:
					m_OnDayEndEvent?.Invoke(this);
					break;
			}
			m_last_time_of_day = current_time_of_day;
		}
	}

	public TimeOfDay GetTimeOfDay()
	{
		if (m_HourOfDay < m_MorningTime)
		{
			return TimeOfDay.Morning;
		}
		else if (m_HourOfDay < m_MorningTime + m_DaylightTime)
		{
			return TimeOfDay.Day;
		}
		else if (m_HourOfDay < m_MorningTime + m_DaylightTime + m_EveningTime)
		{
			return TimeOfDay.Evening;
		}
		else
		{
			return TimeOfDay.Night;
		}
	}
}

public class FMGameLoopManager : MonoBehaviourSingleton<FMGameLoopManager>
{
	public FMDay m_CurrentDay = new FMDay();

	[HideInInspector]
	public List<FMTaskBase> m_TickableTasks = new List<FMTaskBase>();

	public bool m_IsPaused { get; private set; }

	public event System.Action<bool> OnGamePause;

	public void PauseGame(bool value)
	{
		this.m_IsPaused = value;
		this.OnGamePause?.Invoke(m_IsPaused);
	}

	private void Start()
	{
		this.m_IsPaused = false;
	}

	// Update is called once per frame
	void Update()
    {
		if (this.m_IsPaused)
		{
			return;
		}

		m_CurrentDay.IncrementTime(Time.deltaTime);
		if (m_CurrentDay.GetTimeOfDay() != FMDay.TimeOfDay.Day &&
			m_CurrentDay.GetTimeOfDay() != FMDay.TimeOfDay.Evening)
		{
			return;
		}

		for (int i = 0; i < m_TickableTasks.Count; ++i)
		{
			var task = m_TickableTasks[i];
			if (task.m_TaskProcessing)
				m_TickableTasks[i].TickTask(Time.deltaTime);
		}
	}
}
