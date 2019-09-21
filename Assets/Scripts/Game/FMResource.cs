using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMResource : MonoBehaviour
{
	// type
	public int m_ResourceIndex = 0;

	public float m_Amount = 1f;
	public float m_ProcessedAmount = 0f;

	// quality (determined by processor time)
	public float m_QualityDecayRate = 0.05f;
	public float m_Sickness = 1f;

	public void TickDecrementQuality(float time)
	{
		m_Sickness -= time * m_QualityDecayRate;
	}

	private void Start()
	{
		GetComponent<MeshRenderer>().material.color = FMBoardReferences.GetOrCreateInstance().m_ColorsForResource[m_ResourceIndex];
	}

}
