using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMAudienceSpawner : MonoBehaviour
{
    public GameObject prefab;

    public FMStagingArea staging;

    void Awake()
    {
        UnityChatManagerScript.GetOrCreateInstance().OnLogInMessage += FMAudienceSpawner_OnLogInMessage;
        UnityChatManagerScript.GetOrCreateInstance().OnLogOutMessage += FMAudienceSpawner_OnLogOutMessage;
    }

    private void FMAudienceSpawner_OnLogOutMessage(string username)
    {
        var go = GameObject.Find("user+" + username);
        if (go)
        {
            Destroy(go);
        }
    }

    private void FMAudienceSpawner_OnLogInMessage(string username)
    {
        GameObject go = Instantiate(prefab);
        go.name = "user+" + username;
        staging.AddToStaging(go.transform);
    }

    void Update()
    {
        
    }
}
