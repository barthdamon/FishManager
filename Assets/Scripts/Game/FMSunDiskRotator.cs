using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMSunDiskRotator : MonoBehaviour
{
	public bool m_is_daylight_rotator = true;
	public float m_total_rotation_angle = 270f;
	private FMDay m_day;

    void Awake()
    {
		m_day = FMGameLoopManager.GetOrCreateInstance().m_CurrentDay;
	}

    // Update is called once per frame
    void Update()
    {
        if (m_is_daylight_rotator &&
			(m_day.GetTimeOfDay() == FMDay.TimeOfDay.Morning ||
			m_day.GetTimeOfDay() == FMDay.TimeOfDay.Day))
		{
			this.transform.localRotation = Quaternion.AngleAxis(this.m_total_rotation_angle * m_day.GetNormalisedTimeOfDay(), Vector3.forward);
		}
		else if (!m_is_daylight_rotator &&
			(m_day.GetTimeOfDay() == FMDay.TimeOfDay.Evening ||
			m_day.GetTimeOfDay() == FMDay.TimeOfDay.Night))
		{
			this.transform.localRotation = Quaternion.AngleAxis(this.m_total_rotation_angle * m_day.GetNormalisedTimeOfNight(), Vector3.forward);
		}
    }
}
