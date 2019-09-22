using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FM_AnimalBrain : MonoBehaviour
{
    public SpriteRenderer bone;

    public AnimationCurve twitch;

    void OnEnable()
    {
        StartCoroutine(UpdateAnimation());
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
    /*
    //ToDo: perform AI, and also animation (eye twitching, changing sprite based on movement)
    void FixedUpdate()
    {
        UpdateAI();
    }

    void UpdateAI()
    {

    }
    */

    IEnumerator UpdateAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            //random movements
            //if (UnityEngine.Random.value < 0.02f)
            //    StartCoroutine(Move(bone));

            if (UnityEngine.Random.value < 0.03f)
                StartCoroutine(Bounce(bone, Random.Range(0.1f, 2.0f)));

            //random talk
            //if (UnityEngine.Random.value < 0.001f)
            //    if (DialogManager.Get().randomText.Count > 0)
            //        DialogManager.Get().Say_3D(DialogManager.Get().randomText[UnityEngine.Random.Range(0, DialogManager.Get().randomText.Count)], this.gameObject);

        }

        yield break;
    }

    private IEnumerator Bounce(SpriteRenderer bone, float time)
    {
        if (bone.tag == "busy") yield break;

        bone.tag = "busy";
        Vector3 scale = bone.transform.localScale;

        for (float t = 0; t < 1.0f; t += Time.deltaTime / time)
        {
            float f = twitch.Evaluate(t);
            bone.transform.localScale = Vector3.Scale(scale, new Vector3(1, f, 1));
            yield return new WaitForSeconds(0.01f);
        }
        bone.transform.localScale = scale;
        bone.tag = "Untagged";
    }

}
