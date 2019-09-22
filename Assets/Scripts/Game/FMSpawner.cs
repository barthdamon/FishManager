using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;
    public int numberToSpawn = 2;
	public int spawnPerDay = 2;
	public int maxFish = 10;

	private int cumulativeCount = 0;

    public Vector3 randomPosRange = new Vector3(5, 0, 5);

    void Awake()
    {
		FMGameLoopManager.GetOrCreateInstance().m_CurrentDay.m_OnMorningStartEvent += OnSpawnMore;
	}

	private void OnSpawnMore(FMDay day)
	{
		if (cumulativeCount <= maxFish)
		{
			for (int i = 0; i < numberToSpawn; i++)
			{
				Vector3 offset = Vector3.Scale(Random.insideUnitSphere, randomPosRange);
				GameObject go = Instantiate(prefab, parent.position + offset, parent.rotation, parent);
				go.name = "spawn" + (cumulativeCount + i);
				go.GetComponent<FMWorker>().GoToWorkerPool();
			}
			cumulativeCount += numberToSpawn;
		}
	}

    void Update()
    {
        
    }
}
