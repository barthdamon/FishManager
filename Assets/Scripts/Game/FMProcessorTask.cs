using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FMProcessorTask : FMTaskBase
{
	public float m_ProcessScalar = 1f;
	public float m_SingleWorkerProcessTime = 1f;

	public RectTransform m_ProcessingStagingArea;

	public override void TickTask(float time)
	{
		if (m_AssignedWorkers.Count > 0 && m_Resources.Count > 0)
		{
			base.TickTask(time);
			foreach (var item in m_Resources.Skip(1))
			{
				item.TickDecrementQuality(time);
			}
		}
	}

	public override float GetTimeToTrigger()
	{
		// this is dependent on the number of workers you have as well as the tick rate...
		return 3f;
		//return m_AssignedWorkers.Count * m_ProcessWorkerScaleTime;
	}

	public void ProcessNewResource(FMResource resource)
	{
		// todo: animate it to the processing plant?
		m_Resources.Enqueue(resource);
		resource.gameObject.transform.position = m_ProcessingStagingArea.position;
		resource.gameObject.SetActive(true);
	}

	protected override void TriggerTask()
	{
		base.TriggerTask();

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
			if (resource.m_Amount <= 0f)
			{
				// done processing
				FMResource processedResource = m_Resources.Dequeue();
				FMBoardReferences.GetOrCreateInstance().m_ResourceSinks[resource.m_ResourceIndex].m_Resources.Enqueue(processedResource);
			}
		}
	}
}
