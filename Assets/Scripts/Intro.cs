using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public GameObject[] dialogs;

    void Start()
    {
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    { 
        for(int i=0; i<dialogs.Length; i++)
        {
            dialogs[i].SetActive(true);

            while(dialogs[i].activeSelf)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

    }

}
