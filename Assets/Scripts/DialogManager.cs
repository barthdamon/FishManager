using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager singleton;

    public GameObject prefab;
    public Transform parentToSpawnUnder;

    public List<GameObject> instances = new List<GameObject>();

    public List<string> randomText = new List<string>();

    public List<DialogFish> dialogFishes;

    private void Awake()
    {
        singleton = this;
        UnityChatManagerScript.GetOrCreateInstance().OnMessage += DialogManager_OnMessage;
    }
    public static DialogManager Get()
    {
        if (singleton == null)
        {
            GameObject go = new GameObject("DialogManager");
            singleton = go.AddComponent<DialogManager>();
        }
        return singleton;
    }


    private void DialogManager_OnMessage(string username, string message)
    {
        Say_2D(message, username);
    }

    public void Say_2D(string text, string speakername)
    {
        DialogFish df = dialogFishes.Find(d => d.nameTag != null && d.nameTag.text == speakername);

        if (df == null)
        {
            List<DialogFish> dfs = dialogFishes.FindAll(d => d.gameObject.activeSelf == false);
            if (dfs != null && dfs.Count > 0)
                df = dfs[Random.Range(0, 1000) % dfs.Count];
        }

        if (df != null)
        {
            df.Say(text, speakername);
        }
    }

    public void Say_3D(string text, GameObject speaker)
    {
        //gc
        instances.RemoveAll(i => i == null);

        GameObject go = null;
        go = instances.Find(i => i.GetComponent<UI_Over_3D>().target == speaker.transform);

        if (go == null)
        {
            go = Instantiate(prefab, parentToSpawnUnder);
            instances.Add(go);
        }

        go.GetComponent<UI_Over_3D>().target = speaker.transform;
        go.GetComponentInChildren<DialogReader>().lines.AddRange(text.Split(new char[] { '\n' }));
    }

    private void Update()
    {

    }
}
