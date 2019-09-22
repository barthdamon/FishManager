using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMSillyEffectManager : MonoBehaviour
{
    public GameObject rain_vfx;
    float emission_rate;
    // Start is called before the first frame update
    void Start()
    {
        emission_rate = 0;
        UnityChatManagerScript.GetOrCreateInstance().OnMessage += FMSillyEffectManager_OnMessage;
        StartCoroutine(RainVFXTickDisplay());
    }

    private void FMSillyEffectManager_OnMessage(string username, string message)
    {
        if (message.Contains("rain"))
        {
            emission_rate += 5;
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
}
