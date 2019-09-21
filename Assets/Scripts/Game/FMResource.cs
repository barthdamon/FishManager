using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMResource : MonoBehaviour
{
	public Sprite[] m_ContainerRepresentationPrefabs;
	public Sprite m_ContentRepresentationPrefab;

	public SpriteRenderer m_ContainerRepresentation;
	public SpriteRenderer m_ContentsRepresentation;

	public int m_Size = 1;

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

	public void SetResourceVisible(bool visible)
	{
		m_ContainerRepresentation.enabled = visible;
		m_ContentsRepresentation.enabled = visible;
	}

	private void Start()
	{
		m_ContainerRepresentation.sprite = m_ContainerRepresentationPrefabs[m_Size];
		m_ContentsRepresentation.sprite = m_ContentRepresentationPrefab;
	}

}
