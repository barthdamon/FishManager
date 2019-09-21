using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogFish : MonoBehaviour
{
    public RectTransform fish;
    public GameObject dialog;

    public float time_in = 1.0f;
    public float time_in_wait = 1.0f;
    public float time_dialog_min = 1.0f;
    public float time_out_wait = 1.0f;
    public float time_out = 1.0f;

    public Vector2 offset = new Vector2(0, -100.0f);

    void OnEnable()
    {
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        if (dialog != null)
            dialog.SetActive(false);

        if (time_in > 0)
        {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time_in)
            {
                fish.anchoredPosition = offset * (1.0f - t);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            fish.anchoredPosition = offset * 0.0f;
        }

        yield return new WaitForSeconds(time_in_wait);

        if (dialog != null)
            dialog.SetActive(true);

        yield return new WaitForSeconds(time_dialog_min);

        while (dialog!=null && dialog.activeInHierarchy)
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(time_out_wait);

        if (time_out > 0)
        {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time_out)
            {
                fish.anchoredPosition = offset * t;
                yield return new WaitForEndOfFrame();
            }
        }
        //ToDo: option to disappear or STAY

        //destroy or disable?
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
