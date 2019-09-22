using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    public AudioSource audio_source;
    public AudioClip[] audio_clips;
    public List<string> audio_sourcenames = new List<string>();

    public void play_audio(string name)
    {
        var i = audio_sourcenames.IndexOf(name);

        if(i >= 0 && i<audio_clips.Length)
        {
            audio_source.PlayOneShot(audio_clips[i]);
        }
    }
}
