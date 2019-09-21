using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMSinkLevelDisplay : MonoBehaviour
{

	public Sprite m_LevelDisplaySprite;

	public List<GameObject> m_LevelDisplayObjects = new List<GameObject>();

	private float m_CurrentResourceAmount = 0f;

	private void Awake()
	{
		for (int i = 0; i < m_LevelDisplayObjects.Count; ++i)
		{
			m_LevelDisplayObjects[i].GetComponent<SpriteRenderer>().sprite = m_LevelDisplaySprite;
		}
	}

	private int GetResourceLevel()
	{
		// divide into ten sections...
		int segment = (int)FMResource.m_StartingResourceAmount / 10;
		return Mathf.CeilToInt(m_CurrentResourceAmount / (float)segment);
	}

	public void UpdateLevelDisplay(float amount)
	{
		m_CurrentResourceAmount = amount;
		int resourceLevel = GetResourceLevel();
		for (int i = 0; i < m_LevelDisplayObjects.Count; ++i)
		{
			bool showLevel = i < resourceLevel;
			m_LevelDisplayObjects[i].SetActive(showLevel);
		}
	}
}
