using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMBoardReferences : MonoBehaviourSingleton<FMBoardReferences>
{
	[Tooltip("Index corresponds to resource type")]
	public FMResourceSink[] m_ResourceSinks;

	public GameObject[] m_ResourcePrefabs;

	public FMProcessorTask m_Processor;
}
