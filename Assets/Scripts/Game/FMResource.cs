using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FMResourceRepresentation
{
	public GameObject m_RepresentationRoot;
	public SpriteRenderer m_ContainerRepresentation;
	public SpriteRenderer[] m_ContentsRepresentations;
}

public class FMResource : MonoBehaviour
{
	public Sprite m_ContentRepresentationPrefab;

	public FMResourceRepresentation[] m_ResourceRepresentations;
	[HideInInspector]
	private FMResourceRepresentation m_RelevantRepresentation;

	public int m_Size = 1;

	// type
	public int m_ResourceIndex = 0;
	public float m_StartProcessingAmount = 0f;
	public float m_Amount = 1f;
	public float m_ProcessedAmount = 0f;

	// quality (determined by processor time)
	public float m_QualityDecayRate = 0.05f;
	public float m_Sickness = 1f;

	public void TickDecrementQuality(float time)
	{
		m_Sickness -= time * m_QualityDecayRate;
	}

	public void SetResourceVisible(bool visible)
	{
		GetComponentInChildren<Image>().enabled = visible;
		if (m_RelevantRepresentation != null)
		{
			m_RelevantRepresentation.m_ContainerRepresentation.enabled = visible;
			for (int i = 0; i < m_RelevantRepresentation.m_ContentsRepresentations.Length; ++i)
			{
				m_RelevantRepresentation.m_ContentsRepresentations[i].enabled = visible;
			}
		}
	}

	public void SetRepresentation()
	{
		m_RelevantRepresentation = m_ResourceRepresentations[m_Size];
		for (int i = 0; i < m_RelevantRepresentation.m_ContentsRepresentations.Length; ++i)
		{
			m_RelevantRepresentation.m_ContentsRepresentations[i].sprite = m_ContentRepresentationPrefab;
		}

		for (int i = 0; i < m_ResourceRepresentations.Length; ++i)
		{
			if (i != m_Size)
			{
				m_ResourceRepresentations[i].m_RepresentationRoot.SetActive(false);
			}
			else
			{
				m_ResourceRepresentations[i].m_RepresentationRoot.SetActive(true);
			}
		}
	}

}
