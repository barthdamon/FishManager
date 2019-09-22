using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FMProcessorTask : FMTaskBase
{
	public float m_ProcessScalar = 50f;
	public float m_SingleWorkerProcessTime = 1f;
	public float m_StagingAreaRadus = 3f;

	public RectTransform m_ProcessingStagingArea;

	public override void TickTask(float time)
	{
		if (m_AssignedWorkers.Count > 0 && m_Resources.Count > 0)
		{
			foreach (var item in m_Resources.Skip(1))
			{
				item.TickDecrementQuality(time);
			}

			FMResource resource = m_Resources.Peek();
			if (resource != null)
			{
				var totalProcessAmount = 0f;
				for (int i = 0; i < m_AssignedWorkers.Count; ++i)
				{
					totalProcessAmount += m_ProcessScalar * m_AssignedWorkers[i].GetProductivity();
				}

				totalProcessAmount = Mathf.Min(totalProcessAmount, resource.m_Amount);
				resource.m_ProcessedAmount += totalProcessAmount;
				resource.m_Amount -= totalProcessAmount;
			}
			base.TickTask(time);
		}
	}

	protected override bool ShouldTriggerTask()
	{
		FMResource resource = m_Resources.Peek();
		return resource.m_Amount <= 0f;
	}

	protected override float GetDisplayProgress()
	{
		FMResource resource = m_Resources.Peek();
		return resource.m_ProcessedAmount / resource.m_StartProcessingAmount;
	}

	public void ProcessNewResource(FMResource resource)
	{
		// todo: animate it to the processing plant?
		m_Resources.Enqueue(resource);
		Vector2 centerOffset = new Vector2(Random.Range(0f, m_StagingAreaRadus), Random.Range(0f, m_StagingAreaRadus));
		resource.gameObject.transform.SetParent(m_ProcessingStagingArea, false);
		resource.gameObject.transform.localPosition = new Vector3(centerOffset.x, 0f, centerOffset.y);
		resource.m_StartProcessingAmount = resource.m_Amount;
		resource.SetResourceVisible(true);
		resource.m_SmellyFishParticles.SetActive(true);
	}

	protected override void TriggerTask()
	{
		base.TriggerTask();
		FMResource resource = m_Resources.Peek();
		if (resource != null)
		{
			// done processing
			FMResource processedResource = m_Resources.Dequeue();
			processedResource.SetResourceVisible(false);
			FMBoardReferences.GetOrCreateInstance().m_ResourceSinks[resource.m_ResourceIndex].AddResourceToSink(processedResource);
			SetProgress(0f);
		}
	}

	protected override void ShutDown()
	{
		base.ShutDown();
		m_AssignedWorkers.Clear();
	}
}
