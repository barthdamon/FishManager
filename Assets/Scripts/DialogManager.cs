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
}
