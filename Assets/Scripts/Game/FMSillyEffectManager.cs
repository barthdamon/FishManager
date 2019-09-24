using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FMSillyEffectManager : MonoBehaviour
{
    public GameObject rain_vfx;
    float emission_rate;

    string[] unity_ask = 
    {
        "Where do sick fish go?", 
        "Why did the little boy not eat his sushi?",
        "What are fish that act in movies called?",
        "How do you tuna fish?",
        "How do you communicate with a fish you haven’t seen in ages?",
        "Who was the standout musician in the fish band?",
        "Who was the best employee at the balloon factory?",
        "If you can think of a better fish pun…",
        "Did you hear about the newlywed shark couple?",
    };

    string[] unity_answer =
    {
        "To see a sturgeon",
        "Because it looked too fishy.",
        "Starfish.",
        "Adjust their scales.",
        "Drop them a line.",
        "The bass player.",
        "The blow fish.",
        "Let minnow.",
        "They are swimming along nicely.",
    };

    Coroutine unity_ask_silly_questions;

    // Start is called before the first frame update
    void Start()
    {
        emission_rate = 0;
        UnityChatManagerScript.GetOrCreateInstance().OnLogInMessage += FMSillyEffectManager_OnLogInMessage;
        UnityChatManagerScript.GetOrCreateInstance().OnLogOutMessage += FMSillyEffectManager_OnLogOutMessage;
        UnityChatManagerScript.GetOrCreateInstance().OnMessage += FMSillyEffectManager_OnMessage;
        StartCoroutine(RainVFXTickDisplay());
    }
    private void FMSillyEffectManager_OnLogInMessage(string username)
    {
        // Unity logged in
        if (username.ToLower() == ("@" + UnityChatManagerScript.GetOrCreateInstance().get_myname().ToLower()))
        {
            if (unity_ask_silly_questions == null)
            {
                unity_ask_silly_questions = StartCoroutine(UnityAskSillyQuestions());
            }
        }
    }
    private void FMSillyEffectManager_OnLogOutMessage(string username)
    {
        // Unity logged in
        if (username.ToLower() == ("@" + UnityChatManagerScript.GetOrCreateInstance().get_myname().ToLower()))
        {
            if (unity_ask_silly_questions != null)
            {
                StopCoroutine(unity_ask_silly_questions);
            }
        }
    }
    private void FMSillyEffectManager_OnMessage(string username, string message)
    {
        if (message.ToLower().Contains("rain"))
        {
            emission_rate += 20;
        }

        if (message.ToLower().Contains("shake"))
        {
            GetComponent<CameraShake>().ShakeCamera(0.25f, 0.025f);
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

    private IEnumerator UnityAskSillyQuestions()
    {
        var q_or_a = 0;
        int i = Random.Range(0, unity_ask.Length);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(30f, 45f));

            string str;
            if(q_or_a == 0)
            {
                str = unity_ask[i];
            }
            else
            {
                str = unity_answer[i];
            }

            UnityChatManagerScript.GetOrCreateInstance().SendChatMessage(str);
            q_or_a = ++q_or_a % 2;
            if(q_or_a == 0)
            {
                i = ++i % unity_ask.Length;
            }
        };
        // Create
    }
}
