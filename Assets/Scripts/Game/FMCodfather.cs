using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMCodfather : MonoBehaviourSingleton<FMCodfather>
{
	public GameObject m_MafiaPrefab;
	public List<string> buffer = new List<string>();

	public bool m_ShowingMafia = false;

	public void Buffer(string text)
	{
		buffer.Add(text);
	}

	private IEnumerator SpawnMafia()
	{
		m_ShowingMafia = true;
		var canvas = GetComponentInParent<Canvas>();
		var mafiaInstance = Instantiate(m_MafiaPrefab, canvas.transform);
		// get a random sink and put mafia there...
		var potentialSinks = FMBoardReferences.GetOrCreateInstance().m_ResourceSinks;
		int randomSink = Random.Range(0, potentialSinks.Length);
		var sink = potentialSinks[randomSink];
		yield return new WaitForSeconds(5f);

	}

	private void Update()
	{
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

			if (!m_ShowingMafia && message.Contains("mafia") || message.Contains("Mafia")
			{
				StartCoroutine(SpawnMafia());
			}
		}
	}

}
