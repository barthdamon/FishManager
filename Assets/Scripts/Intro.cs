using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public float camerazoom_out_speed; // Denes value: 1.0/9.0

    public GameObject[] dialogs;

    public GameObject title;

    public DialogFish[] speakers;

    public CameraController camCon;

    public GameObject skip_intro_button;
    private bool skip_intro = false;

    void Start()
    {
        skip_intro_button.SetActive(false);
        skip_intro_button.GetComponent<Button>().onClick.AddListener(() => { skip_intro = true; });
		StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        camCon.enabled = false;
        //pause game
        Time.timeScale = 0.0f;

        for(int i=0; i<dialogs.Length; i++)
        {
            if (skip_intro)
            {
                skip_intro = false;
                skip_intro_button.SetActive(false);
                break;
            }
            dialogs[i].SetActive(true);
            skip_intro_button.SetActive(true);

            while (dialogs[i].activeSelf)
            {
                if (skip_intro)
                {
                    dialogs[i].SetActive(false);
                    break;
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

		// Clear out any lingering dialog
		for (int i = 0; i < dialogs.Length; i++)
		{
			var reader = dialogs[i].GetComponentInChildren<DialogReader>();
			if (reader)
				reader.Clear();
		}

		title.SetActive(true);
        skip_intro_button.SetActive(false);

        camCon.enabled = true;

        for (float f = 0.0f; f<0.5f; f += Time.unscaledDeltaTime * camerazoom_out_speed)
        {
            camCon.UpdateZoom(f);
            yield return new WaitForSecondsRealtime(0.01f);
        }

        //unpause
        Time.timeScale = 1.0f;

		Tutorial.GetOrCreateInstance().ShowStartingTutorial();
	}
}
