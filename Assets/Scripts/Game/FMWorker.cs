using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMWorker : MonoBehaviour
{
	private float m_TimeToReturnToWork = 1f;

	// productivity level (determined by food quality)
	float m_SicknessLevel = 0f;

    // Start is called before the first frame update
	public void GoHome()
	{
		m_TimeToReturnToWork *= m_SicknessLevel;
	}

	public void GetSomeGrub()
	{
		int randomSink = Random.Range(0, 3);
		var sink = FMBoardReferences.GetOrCreateInstance().m_ResourceSinks[randomSink];
		m_SicknessLevel = sink.m_CurrentSicknessLevel;
	}

	public void GoToWorkerPool()
	{
	}

	public void TickReturnToWork(float time)
	{
		// if ..
		GoToWorkerPool();
	}

	public float GetProductivity()
	{
		return m_SicknessLevel;
	}
}
