using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMBoardReferences : MonoBehaviourSingleton<FMBoardReferences>
{
	[Tooltip("Index corresponds to resource type")]
	public FMResourceSink[] m_ResourceSinks;
}
