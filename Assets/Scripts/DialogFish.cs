using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogFish : MonoBehaviour
{
    public RectTransform fish;
    public GameObject dialog;
    public Text nameTag;

    public float time_in = 1.0f;
    public float time_in_wait = 1.0f;
    public float time_dialog_min = 1.0f;
    public float time_out_wait = 1.0f;
    public float time_out = 1.0f;

    public Vector2 offset = new Vector2(0, -100.0f);

    public bool playing;

    void OnEnable()
    {
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        playing = true;

        if (dialog != null)
            dialog.SetActive(false);

        if (time_in > 0)
        {
            for (float t = 0.0f; t < 1.0f; t += Time.unscaledDeltaTime / time_in)
            {
                fish.anchoredPosition = offset * (1.0f - t);
                yield return new WaitForEndOfFrame();
            }
            fish.anchoredPosition = offset * 0.0f;
        }
        else
        {
            fish.anchoredPosition = offset * 0.0f;
        }

        yield return new WaitForSecondsRealtime(time_in_wait);

        if (dialog != null)
            dialog.SetActive(true);

        yield return new WaitForSecondsRealtime(time_dialog_min);

        while (dialog!=null && dialog.activeInHierarchy)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }

        yield return new WaitForSecondsRealtime(time_out_wait);

        if (time_out > 0)
        {
            for (float t = 0.0f; t < 1.0f; t += Time.unscaledDeltaTime / time_out)
            {
                fish.anchoredPosition = offset * t;
                yield return new WaitForEndOfFrame();
            }
            fish.anchoredPosition = offset * 1.0f;
        }
        //ToDo: option to disappear or STAY

        //destroy or disable?
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);

        playing = false;
    }

    public void Clear()
    {
        //find it even if disabled
        DialogReader[] drs = dialog.GetComponentsInChildren<DialogReader>(true);
        drs[0].Clear();
    }

    public void Say(string text, string byName, bool setName = true, bool enable_typing_effect = true)
    {
        Debug.Log(byName + ": " + text);

        if (nameTag != null && setName)
            nameTag.text = byName;

        //find it even if disabled
        DialogReader[] drs = dialog.GetComponentsInChildren<DialogReader>(true);
        //drs[0].lines.AddRange(text.Split(new char[] { '\n' }));
        var lines = text.Split(new char[] { '\n' });
        foreach(string line in lines)
        {
            drs[0].add_dialog_line(line, enable_typing_effect);
        }

        this.gameObject.SetActive(true);
    }

}
