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

    public List<string> buffer = new List<string>();

    public List<DialogFish> dialogFishes;

    private void Awake()
    {
        singleton = this;
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

    public void Say(string text, GameObject speaker)
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

    public void Buffer(string text)
    {
        buffer.Add(text);
    }

    private void Update()
    {
        if (buffer.Count > 0)
        {
            string message = buffer[0];
            string from = "";
            //ToDo: process message
            int n = message.IndexOf(": ");
            if (n>0)
            {
                from = message.Substring(0, n);
                message = message.Substring(n + 1);
            }
            DialogFish df = dialogFishes.Find(d => d.nameTag!=null && d.nameTag.text == from);
            if (df == null)
                df = dialogFishes.Find(d => d.gameObject.activeSelf == false);
            if (df != null)
            {
                df.Say(message, from);
                buffer.RemoveAt(0);
            }
        }
    }
}
