﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogReader : MonoBehaviour, IPointerDownHandler 
{
    public List<string> lines = new List<string>();

    public float readCharsPerSec = 10;
    public float autoclickTime = 5.0f;
    public bool autoScaleHorizontal = false;
    public bool autoScaleVertical = true;
    public bool destroy = true;

    public int currentLine;
    public float readTimer;
    public float clickTimer;

    public Text text;

    public RectTransform rect;

    void OnEnable()
    {
        if (text == null)
            text = GetComponentInChildren<Text>();

        currentLine = -1;
        readTimer = 999;
        clickTimer = 999;

        //have to delay this because the text gets set dynamically AFTER SPAWN, and so i cannot update my text right away
        //Next();

        if (rect == null)
            rect = GetComponentInChildren<RectTransform>();

        rect.gameObject.SetActive(false);
    }

    void OnDisable()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (currentLine<0 || readTimer > lines[currentLine].Length)
        {//all shown, wait for click
            clickTimer += Time.deltaTime;
            if (autoclickTime > 0
                && clickTimer > autoclickTime)
            {
                Next();
            }
        } else
        {
            readTimer += Time.deltaTime * readCharsPerSec;
            text.text = lines[currentLine].Substring(0, (int)readTimer);
            clickTimer = 0;
        }
    }

    void Next()
    {
        if (++currentLine >= lines.Count)
        {
            if (destroy)
                Destroy(this.gameObject);
            else
                this.gameObject.SetActive(false);
            return;
        }

        readTimer = 0;

        if (autoScaleHorizontal || autoScaleVertical)
        {
            //RectTransform rect = GetComponentInChildren<RectTransform>();
            rect.gameObject.SetActive(true);

            float charsPerLine = rect.rect.width / text.fontSize;
            float needLines = 1 + (lines[currentLine].Length / (charsPerLine * 0.8f));

            float width = charsPerLine * text.fontSize;
            float height = needLines * text.fontSize * 1.0f * text.lineSpacing;
            rect.sizeDelta = new Vector2(width, height);

            Debug.Log("CPL: " + charsPerLine);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
        if (readTimer > lines[currentLine].Length)
        {
            Next();
        }
        else
        {
            readTimer = lines[currentLine].Length;
        }
    }
}