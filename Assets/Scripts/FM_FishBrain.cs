using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FM_FishBrain : MonoBehaviour
{
    public NavMeshAgent agent;

    public FMWorker worker;

    public Color healthyColor = Color.white;
    public Color sickColor = Color.green;

    public enum Bone
    {
        Body,
        EyeL,
        EyeR,
        handL,
        handR,
        handL2
    }

    public SpriteRenderer[] bones;   //fishbones :)

    public Transform idleRoot;
    public Transform workingRoot;

    public AnimationCurve hammerCurve;


    void OnEnable()
    {
        worker = GetComponent<FMWorker>();

        StartCoroutine(UpdateAnimation());
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }

    //ToDo: perform AI, and also animation (eye twitching, changing sprite based on movement)
    void FixedUpdate()
    {
        UpdateAI();
    }

    void UpdateAI()
    {

    }

    IEnumerator UpdateAnimation()
    {
        StartCoroutine(Bounce(bones[(int)Bone.Body], UnityEngine.Random.Range(0.1f, 0.3f)));

        if (idleRoot != null && workingRoot != null)
        {
            StartCoroutine(Hammer(bones[(int)Bone.handL2], UnityEngine.Random.Range(0.1f, 0.3f)));
        }

        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            //random eye twitch
            if (UnityEngine.Random.value < 0.02f)
                StartCoroutine(Twitch(bones[(int)Bone.EyeL]));
            if (UnityEngine.Random.value < 0.02f)
                StartCoroutine(Twitch(bones[(int)Bone.EyeR]));

            //random talk
            if (UnityEngine.Random.value < 0.001f)
                if (DialogManager.Get().randomText.Count > 0)
                    DialogManager.Get().Say_3D(DialogManager.Get().randomText[UnityEngine.Random.Range(0,DialogManager.Get().randomText.Count)], this.gameObject);

            if (worker != null)
            {
                bool isWorking = (worker.currentTask != null);
                if (idleRoot != null && workingRoot != null)
                {
                    if (workingRoot.gameObject.activeSelf != isWorking)
                        workingRoot.gameObject.SetActive(isWorking);
                    if (idleRoot.gameObject.activeSelf != !isWorking)
                        idleRoot.gameObject.SetActive(!isWorking);
                }
                bones[(int)Bone.Body].color = Color.Lerp(healthyColor, sickColor, worker.m_SicknessLevel);
            }
        }
        //StopCoroutine(Bounce);
        yield break;
    }

    private IEnumerator Twitch(SpriteRenderer bone)
    {
        if (bone.tag == "busy") yield break;

        bone.tag = "busy";
        Vector3 scale = bone.transform.localScale;
        bone.transform.localScale = Vector3.Scale(scale, new Vector3(1.0f, 0.5f, 1.0f));
        yield return new WaitForSeconds(0.15f);
        bone.transform.localScale = scale;
        bone.tag = "Untagged";
    }

    private IEnumerator Bounce(SpriteRenderer bone, float period = 0.2f)
    {
        float t = 0.0f;
        float dT = 0.02f;
        Vector3 scale = bone.transform.localScale;
        while (true)
        {
            bone.transform.localScale = Vector3.Scale(scale, new Vector3(1.0f, 1.0f + 0.1f * Mathf.Sin(Mathf.PI * t / period), 1.0f));
            yield return new WaitForSeconds(dT);
            t += dT;
        }
        //bone.transform.localScale = scale;
    }
    private IEnumerator Hammer(SpriteRenderer bone, float period = 0.2f)
    {
        //Debug.Log("hammer " + bone.name, bone);
        float t = 0.0f;
        float dT = 0.02f;
        Vector3 rot = bone.transform.localRotation.eulerAngles;
        while (true)
        {
            bone.transform.localRotation = Quaternion.Euler(rot + Vector3.forward* 90.0f *hammerCurve.Evaluate((t/period) % 1.0f));
            yield return new WaitForSeconds(dT);
            t += dT;
        }
    }

}
