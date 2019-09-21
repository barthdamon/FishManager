using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMGeneratorTask : FMTaskBase
{
	public float m_GenerationTimePerPerson = 5f;
	public float m_ResourceCapacityPerWorkerOnTrigger = 100f;

	// multiple generator tasks
	public int m_NumberOfWorkersRequired = 1;
	// #number of workers
	// in order to generate requires x workers

	[HideInInspector]
	public FMWorkerSlotHelper m_BoatSlots;
	[HideInInspector]
	public FMEquipment m_AssignedEquipment;


	private Vector3 m_DockPosition;
	private FMStagingArea m_WorkerStagingArea;


	// Equipment is on the dock, decides what type of fish the boat can get
	// equiement is a worker on the dock and gets assigned to boats to give them a resource destination
	// cannot start until equipment is assigned

	public void SetEquipment(FMEquipment equipment)
	{
		m_BoatSlots.UnassignEquipment(m_AssignedEquipment);
		m_BoatSlots.AssignEquipment(equipment);

		m_AssignedEquipment = equipment;
	}

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
		m_BoatSlots.AssignWorker(worker);
		return CheckProcessing();
	}

	public bool CheckProcessing()
	{
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
			// size is an index
			resourceComponent.m_Size = m_NumberOfWorkersRequired - 1;
			resourceComponent.SetRepresentation();
			m_BoatSlots.AssignResource(resourceComponent);
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
		m_BoatSlots = GetComponentInChildren<FMWorkerSlotHelper>();
	}

	protected override void OnStart()
	{
		base.OnStart();
		m_WorkerStagingArea = FMBoardReferences.GetOrCreateInstance().m_WorkerStagingArea;
		m_DockPosition = transform.position;
	}

	protected override void TriggerTask()
	{
		float tickResourceAmount = 0f;
		for (int i = 0; i < m_AssignedWorkers.Count; ++i)
		{
			float workerProductivity = m_AssignedWorkers[i].GetProductivity() * m_ResourceCapacityPerWorkerOnTrigger;
			tickResourceAmount += workerProductivity;
		}

		if (m_TaskProcessing)
			m_Resources.Peek().m_Amount += tickResourceAmount;

		for (int i = 0; i < m_AssignedWorkers.Count; ++i)
		{
			m_AssignedWorkers[i].GoToWorkerPool();
		}

		for (int i = 0; i < m_AssignedWorkers.Count; ++i)
		{
			m_BoatSlots.UnassignWorker(m_AssignedWorkers[i]);
		}

		m_AssignedWorkers.Clear();
		m_TaskProcessing = false;
		m_TimeSinceLastTrigger = 0f;


		var processor = FMBoardReferences.GetOrCreateInstance().m_Processor;
		var generatedResource = m_Resources.Dequeue();
		SetProgress(0f);
		m_BoatSlots.UnassignResource(generatedResource);
		processor.ProcessNewResource(generatedResource);
	}

	public override void TickTask(float time)
	{
		base.TickTask(time);

	}

	public override float GetTimeToTrigger()
	{
		return m_GenerationTimePerPerson * m_NumberOfWorkersRequired;
	}

	public override void SetProgress(float f)
	{
		base.SetProgress(f);

		if (!m_TaskProcessing || m_AssignedEquipment == null)
			return;

		Vector3 fishingPosition = FMBoardReferences.GetOrCreateInstance().m_ResourceBoatDestinations[m_AssignedEquipment.m_ResourceIndex].position;
		// use the position to the fishing hole as 50%, otherwise come back for the second 50%
		bool gotFish = progress > 0.5f;
		Vector3 destination = gotFish ? m_DockPosition : fishingPosition;
		// on the way there
		// lerp position will b < 0.5 but we want out of 0.5
		float lerpPosition = gotFish ? (progress - 0.5f) / 0.5f : progress / 0.5f;
		Vector3 startPosition = gotFish ? fishingPosition : m_DockPosition;
		transform.position = Vector3.Lerp(startPosition, destination, lerpPosition);

		if (gotFish)
		{
			m_Resources.Peek().SetResourceVisible(true);
		}
	}
}