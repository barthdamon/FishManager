using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public static void Play(string clipName)
    {
        Debug.Log("AUDIO_PLAY " + clipName);
    }
}
