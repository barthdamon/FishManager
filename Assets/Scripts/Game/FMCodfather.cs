using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMCodfather : MonoBehaviourSingleton<FMCodfather>
{
	public GameObject m_MafiaPrefab;
	public bool m_ShowingMafia = false;

	private Canvas m_SpawnCanvas;
	private float m_TimeToSpawnMafia = 20f;
	private float m_LastMafiaSpawnTime = 0f;

	public void Start()
	{
		m_LastMafiaSpawnTime = Time.time;
		m_SpawnCanvas = FindObjectOfType<Canvas>();

        UnityChatManagerScript.GetOrCreateInstance().OnMessage += FMCodfather_OnMessage;
	}

    private void FMCodfather_OnMessage(string username, string message)
    {
        if (!m_ShowingMafia && message.ToLower().Contains("mafia"))
        {
            StartCoroutine(SpawnMafia());
        }
    }

    private IEnumerator SpawnMafia()
	{
		m_ShowingMafia = true;
		var mafiaInstance = Instantiate(m_MafiaPrefab, m_SpawnCanvas.transform);
		// get a random sink and put mafia there...
		//var potentialSinks = FMBoardReferences.GetOrCreateInstance().m_ResourceSinks;
		//int randomSink = Random.Range(0, potentialSinks.Length);
		//var sink = potentialSinks[randomSink];
		var staging = FMBoardReferences.GetOrCreateInstance().m_WorkerPoolStagingArea;
		staging.AddToStaging(mafiaInstance.transform);
		FMPlayer.GetOrCreateInstance().IncrementCapital(-25f);
		//TODO: have the mafia say something
		yield return new WaitForSeconds(5f);
		m_LastMafiaSpawnTime = Time.time;
		Destroy(mafiaInstance);
        m_ShowingMafia = false;
	}

	private void Update()
	{
		if (!m_ShowingMafia && (Time.time - m_LastMafiaSpawnTime > m_TimeToSpawnMafia) &&
			!FMGameLoopManager.GetOrCreateInstance().m_IsPaused)
		{
			StartCoroutine(SpawnMafia());
		}
	}

}
