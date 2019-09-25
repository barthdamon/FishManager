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

    public List<DialogFish> dialogFishes;       //audience

    public List<DialogFish> dialogFishesCast;   //Jim, Bob, Mafia, etc

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

    public void Say_2D(string text, string speakername, bool cast = false)
    {
        List<DialogFish> pool = cast ? dialogFishesCast : dialogFishes;

        DialogFish df = pool.Find(d => (d.nameTag != null && d.nameTag.text == speakername) || d.name == speakername);

        if (df == null)
        {//need a new one
            List<DialogFish> dfs = pool.FindAll(d => d.gameObject.activeSelf == false);
            if (dfs != null && dfs.Count > 0)
            {
                df = dfs[Random.Range(0, 1000) % dfs.Count];
                if (df != null)
                    df.Clear();
            }
        }

        if (df != null)
        {
            // because emoji has glitch with typing effect, so we show the whole message if it comes from the chat
            // only turn on the effect on cutscene fishes
            var enable_typing_effect = cast;
            df.Say(text, speakername, !cast, enable_typing_effect);
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
        //go.GetComponentInChildren<DialogReader>().lines.AddRange(text.Split(new char[] { '\n' }));
        var lines = text.Split(new char[] { '\n' });
        foreach (string line in lines)
        {
            // turn on typing effect always for 3d text bubbles
            var enable_typing_effect = true;
            go.GetComponentInChildren<DialogReader>().add_dialog_line(line, enable_typing_effect);
        }
    }

    private void Update()
    {

    }
}
