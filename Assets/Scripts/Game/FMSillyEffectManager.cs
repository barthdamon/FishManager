using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMSillyEffectManager : MonoBehaviour
{
    public GameObject rain_vfx;
    float emission_rate;

    string[] unity_ask = 
    {
        "Where do sick fish go?", 
        "Why did the little boy not eat his sushi?",
        "If a fish got the main role in a movie, what would it be called?", "Starfish.",
    };

    string[] unity_answer =
    {
        "To see a sturgeon",
        "Because it looked too fishy.",
        "Starfish.",
    };

    // Start is called before the first frame update
    void Start()
    {
        emission_rate = 0;
        UnityChatManagerScript.GetOrCreateInstance().OnMessage += FMSillyEffectManager_OnMessage;
        StartCoroutine(RainVFXTickDisplay());

        //StartCoroutine(UnityAsk());
    }

    private void FMSillyEffectManager_OnMessage(string username, string message)
    {
        if (message.ToLower().Contains("rain"))
        {
            emission_rate += 20;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var ps = rain_vfx.GetComponent<ParticleSystem>();
        var em = ps.emission;

        em.rateOverTimeMultiplier = emission_rate;
    }

    private IEnumerator RainVFXTickDisplay()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.75f));

            emission_rate -= 1.0f;
            emission_rate = Mathf.Max(0, emission_rate);
        };
        // Create
    }

    private IEnumerator UnityAsk()
    {
        var i = 0;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 5f));

            string str = i % 2 == 0 ? unity_ask[i] : unity_answer[i];
            UnityChatManagerScript.GetOrCreateInstance().SendChatMessage(str);
            i++;
            i = i % unity_ask.Length;
        };
        // Create
    }
}
