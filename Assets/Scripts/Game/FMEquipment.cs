using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMEquipment : MonoBehaviour
{
	public int m_ResourceIndex = 0;

	public SpriteRenderer m_SpriteRenderer;
	public Sprite m_ConstructingSprite;
	public Sprite m_ConstructedSprite;

	public FMGeneratorTask m_AssignedTask;
	public bool m_AssignmentCompleted = false;

	public float m_TimeToCompleteAssignment = 5f;

	private float m_CurrentAssignTime = 0f;

	private Color GetColor()
	{
		return FMBoardReferences.GetOrCreateInstance().m_ColorsForResource[m_ResourceIndex];
	}

	private void Awake()
	{
		var draggable = GetComponent<FMDraggable>();
		draggable.m_OnAssignedTask += AssignToTask;
	}

	public void AssignToTask(FMTaskBase task)
	{
		if (task is FMGeneratorTask)
		{
			if (m_AssignedTask != null)
			{
				m_AssignedTask.SetEquipment(null);
			}

			SetAssignmentCompleted(false);
			m_CurrentAssignTime = 0f;
			m_AssignedTask = (FMGeneratorTask)task;
			m_AssignedTask.SetEquipment(this);
			m_AssignedTask.fillerImage.color = GetColor();
		}
	}

	public void Update()
	{
		if (m_AssignedTask != null && !m_AssignmentCompleted)
		{
			m_CurrentAssignTime += Time.deltaTime;
			m_AssignedTask.SetProgress(m_CurrentAssignTime / m_TimeToCompleteAssignment);
			if (m_CurrentAssignTime >= m_TimeToCompleteAssignment)
			{
				m_AssignedTask.SetProgress(0f);
				SetAssignmentCompleted(true);
			}
		}
	}

	private void SetAssignmentCompleted(bool completed)
	{
		m_AssignmentCompleted = completed;
		if (completed)
		{
			m_SpriteRenderer.sprite = m_ConstructedSprite;
		}
		else
		{
			m_SpriteRenderer.sprite = m_ConstructingSprite;
		}
	}

}
