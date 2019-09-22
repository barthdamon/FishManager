using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMPlayer : MonoBehaviourSingleton<FMPlayer>
{
    public delegate void CapitalIncreaseEvent(float value);
    public event CapitalIncreaseEvent OnCapitalIncrease;

	public GameObject m_GameOverUi;

	private bool m_game_over = false;
    
    public float m_Capital
    {
        get;
        private set;
    }

    public void IncrementCapital(float value)
    {
		m_Capital += value;
        OnCapitalIncrease?.Invoke(value);

		if (m_Capital <= 0f && !m_game_over)
		{
			Time.timeScale = 0f;
			m_game_over = true;
			m_Capital = 0f;
			FMGameLoopManager.GetOrCreateInstance().PauseGame(true);
			DialogManager.Get().Say_2D("Your time is up my friend...", "DialogMafia", true);
			DialogManager.Get().Say_2D("I gave you a chance to make things right...", "DialogMafia", true);
			DialogManager.Get().Say_2D("... But you've Haddock.", "DialogMafia", true);
			DialogManager.Get().Say_2D("It's time you go back to sleeping with the fishes.", "DialogMafia", true);
			DialogManager.Get().Say_2D("Boys! Feed him to the dog fish...", "DialogMafia", true);

			StartCoroutine(DoCoroutine());
		}
    }

	private IEnumerator DoCoroutine()
	{
		yield return new WaitForSecondsRealtime(37f);

		m_GameOverUi?.SetActive(true);
	}

	private void Awake()
    {
        m_Capital = 100f;
    }
}
