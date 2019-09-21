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

	public FMStagingArea m_WorkerStagingArea;
}
