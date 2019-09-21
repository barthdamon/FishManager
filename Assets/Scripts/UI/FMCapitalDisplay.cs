using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMCapitalDisplay : MonoBehaviour
{
	public Text m_CapitalDisplayText;

    // Update is called once per frame
    void Update()
    {
		m_CapitalDisplayText.text = FMPlayer.GetOrCreateInstance().m_Capital.ToString();
	}
}
