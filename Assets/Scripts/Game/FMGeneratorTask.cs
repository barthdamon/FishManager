using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMGeneratorTask : FMTaskBase
{
	public float m_GenerationTimePerPerson = 1f;
	public float m_ResourceCapacityPerWorkerPerTick = 1f;

	// multiple generator tasks
	public int m_NumberOfWorkersRequired = 1;
	// #number of workers
	// in order to generate requires x workers

	public FMEquipment m_AssignedEquipment;


	// Equipment is on the dock, decides what type of fish the boat can get
	// equiement is a worker on the dock and gets assigned to boats to give them a resource destination
	// cannot start until equipment is assigned

	// completion time * worker productivity
	// outputs resources when completed onto the processor task
	public override bool AcceptsWorkers()
	{
		//var acceptsWorkers = m_AssignedEquipment != null && m_AssignedEquipment.m_AssignmentCompleted;
		//acceptsWorkers &= m_AssignedWorkers.Count < m_NumberOfWorkersRequired;
		// can accept workers while upgrading?
		return m_AssignedWorkers.Count < m_NumberOfWorkersRequired;
	}

	public override bool AssignWorker(FMWorker worker)
	{
		base.AssignWorker(worker);
		bool nowProcessing = m_AssignedWorkers.Count >= m_NumberOfWorkersRequired;
		nowProcessing &= m_AssignedEquipment != null && m_AssignedEquipment.m_AssignmentCompleted;
		if (!m_TaskProcessing && nowProcessing)
		{
			// todo: send to a specific resource type depending on where boat is directed
			var resourcePrefab = FMBoardReferences.GetOrCreateInstance().m_ResourcePrefabs[m_AssignedEquipment.m_ResourceIndex];
			var canvas = GetComponentInParent<Canvas>();
			var resourceInstance = Instantiate(resourcePrefab, canvas.transform);
			resourceInstance.transform.localScale = Vector3.one;
			resourceInstance.transform.position = transform.position;
			var resourceComponent = resourceInstance.GetComponent<FMResource>();
			resourceComponent.m_Size = m_NumberOfWorkersRequired;
			resourceComponent.SetRepresentation();
			resourceComponent.SetResourceVisible(false);
			m_Resources.Enqueue(resourceComponent);
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

	private void Awake()
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

		var processor = FMBoardReferences.GetOrCreateInstance().m_Processor;
		processor.ProcessNewResource(m_Resources.Dequeue());
	}

	public override void TickTask(float time)
	{
		base.TickTask(time);
		float tickResourceAmount = 0f;
		for (int i = 0; i < m_AssignedWorkers.Count; ++i)
		{
			float workerProductivity = m_AssignedWorkers[i].GetProductivity() * m_ResourceCapacityPerWorkerPerTick;
			tickResourceAmount += workerProductivity;
		}

		if (m_TaskProcessing)
			m_Resources.Peek().m_Amount += tickResourceAmount;
	}

	public override float GetTimeToTrigger()
	{
		return m_GenerationTimePerPerson * m_NumberOfWorkersRequired;
	}
}
