using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMRestaurantCrowd : MonoBehaviour
{

	public int m_CrowdSize = 30;
	public GameObject m_CitizenFishPrefab;

	public List<GameObject> m_Citizens = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
		SpawnCrowd();
		StartCoroutine(UpdateCrowd());
	}

	public void SpawnCrowd()
	{
		var sinks = FMBoardReferences.GetOrCreateInstance().m_ResourceSinks;
		var canvas = GetComponentInParent<Canvas>();
		// spawn equally between the three sinks...
		for (int i = 0; i < m_CrowdSize; ++i)
		{
			// assign evenly between three pools to start
			var newCitizen = Instantiate(m_CitizenFishPrefab, canvas.transform);
			var sinkIndexToUse = i < 10 ? 0 : i < 20 ? 1 : 2;
			var stagingArea = sinks[sinkIndexToUse].m_WorkerStagingArea;
			stagingArea.AddToStaging(newCitizen.transform);
			m_Citizens.Add(newCitizen);
		}
	}

	private IEnumerator UpdateCrowd()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			var sinks = FMBoardReferences.GetOrCreateInstance().m_ResourceSinks;

			// want the ratio of supply between sinks
			var totalAmounts = 0f;
			float[] sinkAmounts = new float[sinks.Length];
			for (int i = 0; i < sinks.Length; ++i)
			{
				float sum = sinks[i].GetCurrentResourceSum();
				sinkAmounts[i] = sum;
				totalAmounts += sum;
			}

			float[] sinkPercentages = new float[sinks.Length];
			for (int i = 0; i < sinks.Length; ++i)
			{
				sinkPercentages[i] = sinkAmounts[i] / totalAmounts;
			}

			int[] sinkIndexCutoffs = new int[sinks.Length];
			for (int i = 0; i < sinks.Length; ++i)
			{
				int thisSinkIndices = (int)(sinkPercentages[i] * m_CrowdSize);
				int previousCutoff = i == 0 ? 0 : sinkIndexCutoffs[i - 1];
				sinkIndexCutoffs[i] = previousCutoff + thisSinkIndices;
			}

			for (int i = 0; i < m_Citizens.Count; ++i)
			{
				int sinkIndexToGoTo = 0;
				for (int c = 0; c < sinkIndexCutoffs.Length; ++c)
				{
					if (i < sinkIndexCutoffs[c])
					{
						sinkIndexToGoTo = c;
						break;
					}
				}

				sinks[sinkIndexToGoTo].m_WorkerStagingArea.AddToStaging(m_Citizens[i].transform);
			}

			// TODO: adjust to the popular one slowly, not all at once in a sudden reaction
		}
	}
}
