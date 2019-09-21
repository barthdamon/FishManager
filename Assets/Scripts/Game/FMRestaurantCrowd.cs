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
			var potentialSinks = FMBoardReferences.GetOrCreateInstance().m_ResourceSinks;
			var sinks = new List<FMResourceSink>();
			// if there is at least 1 amount there then it is valid...
			for (int i = 0; i < potentialSinks.Length; ++i)
			{
				if (potentialSinks[i].GetCurrentResourceSum() > 0)
					sinks.Add(potentialSinks[i]);
			}

			// want the ratio of supply between sinks
			var totalAmounts = 0f;
			float[] sinkAmounts = new float[sinks.Count];
			for (int i = 0; i < sinks.Count; ++i)
			{
				float sum = sinks[i].GetCurrentResourceSum();
				sinkAmounts[i] = sum;
				totalAmounts += sum;
			}

			float[] inverseSinkPercentages = new float[sinks.Count];
			float totalInversePercentages = 0f;
			for (int i = 0; i < sinks.Count; ++i)
			{
				var inversePercentage = 1 - (sinkAmounts[i] / totalAmounts);
				inverseSinkPercentages[i] = inversePercentage;
				totalInversePercentages += inversePercentage;
			}

			// then if we get the percentages of this we can run through the index cutoffs...
			float[] sinkPercentages = new float[sinks.Count];
			for (int i = 0; i < sinks.Count; ++i)
			{
				sinkPercentages[i] = (inverseSinkPercentages[i] / totalInversePercentages);
			}

			int[] sinkIndexCutoffs = new int[sinks.Count];
			for (int i = 0; i < sinks.Count; ++i)
			{
				int thisSinkIndices = (int)(sinkPercentages[i] * m_CrowdSize);
				int previousCutoff = i == 0 ? 0 : sinkIndexCutoffs[i - 1];
				sinkIndexCutoffs[i] = previousCutoff + thisSinkIndices;
			}

			// flock proportionately to the ones with the least %

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
