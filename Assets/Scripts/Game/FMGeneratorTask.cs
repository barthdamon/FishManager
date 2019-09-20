using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMTaskGenerator : FMTaskBase
{
	public FMProcessorTask m_Processor;

	public float m_GenerationTimePerPerson;
	public float m_ResourceCapacityPerWorker;

	// multiple generator tasks
	public int m_NumberOfWorkersRequired;
	// #number of workers
	// in order to generate requires x workers

	// completion time * worker productivity
	// outputs resources when completed onto the processor task
	public override bool AssignWorker(FMWorker worker)
	{
		base.AssignWorker(worker);
		bool nowProcessing = m_AssignedWorkers.Count >= m_NumberOfWorkersRequired;
		if (!m_TaskProcessing && nowProcessing)
		{
			m_Resources.Enqueue(new FMResource());
		}
		m_TaskProcessing = nowProcessing;
		return m_TaskProcessing;
	}

	public override bool RemoveWorker(FMWorker worker)
	{
		base.RemoveWorker(worker);
		m_TaskProcessing = m_AssignedWorkers.Count >= m_NumberOfWorkersRequired;
		m_TimeSinceLastTrigger = 0f;
		return m_TaskProcessing;
	}

	private void Start()
	{
		m_TaskProcessing = false;
	}


	protected override void TriggerTask()
	{
		for (int i = 0; i < m_AssignedWorkers.Count; ++i)
		{
			m_AssignedWorkers[i].GoToWorkerPool();
		}
		m_AssignedWorkers.Clear();
		m_TaskProcessing = false;
		m_TimeSinceLastTrigger = 0f;

		m_Processor.m_Resources.Enqueue(m_Resources.Dequeue());
	}

	public override void TickTask(float time)
	{
		base.TickTask(time);
		float tickResourceAmount = 0f;
		for (int i = 0; i < m_AssignedWorkers.Count; ++i)
		{
			float workerProductivity = m_AssignedWorkers[i].GetProductivity() * m_ResourceCapacityPerWorker;
			tickResourceAmount += workerProductivity;
		}
		m_Resources.Peek().m_Amount += tickResourceAmount;
	}

	public override float GetTimeToTrigger()
	{
		return m_GenerationTimePerPerson * m_NumberOfWorkersRequired;
	}
}
