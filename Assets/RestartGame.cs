using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
	{
        UnityChatManagerScript.GetOrCreateInstance().DisconnectFromChat();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
