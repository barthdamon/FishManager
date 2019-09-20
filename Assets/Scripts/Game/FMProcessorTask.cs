using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FMProcessorTask : FMTaskBase
{


	public float m_ProcessScalar = 1f;

	public override void TickTask(float time)
	{
		base.TickTask(time);
		foreach (var item in m_Resources.Skip(1))
		{
			item.TickDecrementQuality(time);
		}
	}

	public override float GetTimeToTrigger()
	{
		return 3f;
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
