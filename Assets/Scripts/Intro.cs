using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public GameObject[] dialogs;

    public GameObject title;

    public DialogFish[] speakers;

    public CameraController camCon;

    void Start()
    {
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        camCon.enabled = false;

        //pause game
        Time.timeScale = 0.0f;

        for(int i=0; i<dialogs.Length; i++)
        {
            dialogs[i].SetActive(true);

            while(dialogs[i].activeSelf)
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        title.SetActive(true);

        camCon.enabled = true;

        for (float f = 0.0f; f<0.4f; f += Time.unscaledDeltaTime / 8.0f)
        {
            camCon.UpdateZoom(f);
            yield return new WaitForSecondsRealtime(0.01f);
        }

        //unpause
        Time.timeScale = 1.0f;
    }

}
