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


        for (float t = 0.0f; t<1.0f; t += Time.deltaTime / time_in)
        {
            fish.anchoredPosition = offset * (1.0f-t);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(time_in_wait);

        if (dialog != null)
            dialog.SetActive(true);

        while (dialog!=null && dialog.activeInHierarchy)
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(time_out_wait);

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time_out)
        {
            fish.anchoredPosition = offset * t;
            yield return new WaitForEndOfFrame();
        }

        //destroy or disable?
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
