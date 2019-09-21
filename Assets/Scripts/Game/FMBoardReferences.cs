using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMBoardReferences : MonoBehaviourSingleton<FMBoardReferences>
{
	[Tooltip("Index corresponds to resource type")]
	public FMResourceSink[] m_ResourceSinks;
	[Tooltip("Index corresponds to resource type")]
	public GameObject[] m_ResourcePrefabs;

	public FMProcessorTask m_Processor;

	public Color[] m_ColorsForResource;

	[Tooltip("Index corresponds to resource type")]
	public Transform[] m_ResourceBoatDestinations;

	[Tooltip("The Staging area to which workers will be returned when they are ready to work.")]
	public FMStagingArea m_WorkerPoolStagingArea;

	[Tooltip("The Staging area to which workers will be go at the end of the day.")]
	public FMStagingArea m_WorkerHomeStagingArea;

	public FMStagingArea m_WorkerStagingArea;
	public FMStagingArea m_EquipmentStagingArea;
}
