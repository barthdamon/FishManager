﻿using System;
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
        handR
    }

    public SpriteRenderer[] bones;   //fishbones :)


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

        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            //random eye twitch
            if (UnityEngine.Random.value < 0.02f)
                StartCoroutine(TwitchEye(bones[(int)Bone.EyeL]));
            if (UnityEngine.Random.value < 0.02f)
                StartCoroutine(TwitchEye(bones[(int)Bone.EyeR]));

            //random talk
            if (UnityEngine.Random.value < 0.001f)
                if (DialogManager.Get().randomText.Count > 0)
                    DialogManager.Get().Say_3D(DialogManager.Get().randomText[UnityEngine.Random.Range(0,DialogManager.Get().randomText.Count)], this.gameObject);

            if (worker != null)
            {
                if (worker.currentTask != null)
                {

                }
                else
                {

                }

                bones[(int)Bone.Body].color = Color.Lerp(healthyColor, sickColor, worker.m_SicknessLevel);
            }
        }
        //StopCoroutine(Bounce);
        yield break;
    }

    private IEnumerator TwitchEye(SpriteRenderer bone)
    {
        if (bone.tag == "busy") yield break;

        bone.tag = "busy";
        Vector3 scale = bone.transform.localScale;
        bone.transform.localScale = Vector3.Scale(scale, new Vector3(1.0f, 0.5f, 1.0f));
        yield return new WaitForSeconds(0.15f);
        bone.transform.localScale = scale;
        bone.tag = null;
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
            bone.transform.localScale = scale;
        }
    }

}
