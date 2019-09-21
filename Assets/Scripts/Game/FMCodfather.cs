using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMCodfather : MonoBehaviour
{
	public GameObject m_MafiaPrefab;
	public List<string> buffer = new List<string>();

	public void Buffer(string text)
	{
		buffer.Add(text);
	}

	private void SpawnMafia()
	{

	}

}
