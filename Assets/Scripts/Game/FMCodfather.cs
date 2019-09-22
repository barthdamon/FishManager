using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMCodfather : MonoBehaviourSingleton<FMCodfather>
{
	public GameObject m_MafiaPrefab;
	public List<string> buffer = new List<string>();

	public bool m_ShowingMafia = false;

	private Canvas m_SpawnCanvas;
	private float m_TimeToSpawnMafia = 20f;
	private float m_LastMafiaSpawnTime = 0f;

	public void Buffer(string text)
	{
		buffer.Add(text);
	}

	public void Start()
	{
		m_LastMafiaSpawnTime = Time.time;
		m_SpawnCanvas = FindObjectOfType<Canvas>();
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
	}

	private void Update()
	{
		if (!m_ShowingMafia && (Time.time - m_LastMafiaSpawnTime > m_TimeToSpawnMafia))
		{
			StartCoroutine(SpawnMafia());
		}

		if (buffer.Count > 0)
		{
			string message = buffer[0];
			string from = "";
			//ToDo: process message
			int n = message.IndexOf(": ");
			if (n > 0)
			{
				from = message.Substring(0, n);
				message = message.Substring(n + 1);
			}

			if (!m_ShowingMafia && message.Contains("mafia") || message.Contains("Mafia"))
			{
				StartCoroutine(SpawnMafia());
			}
		}
	}

}
