using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogReader : MonoBehaviour, IPointerDownHandler 
{
    public List<string> lines = new List<string>();

    class DialogLine
    {
        public string line;
        public bool enable_typewriter_effect;
    }
    List<DialogLine> dialog_lines = new List<DialogLine>();

    public float readCharsPerSec = 10;
    public float autoclickTime = 5.0f;
    public bool autoScaleHorizontal = false;
    public bool autoScaleVertical = true;
    public bool destroy = true;

    public int currentLine;
    public float readTimer;
    public float clickTimer;

    public TMPro.TMP_EmojiTextUGUI text;

    public RectTransform rect;

    void Start()
    {
        foreach(var l in lines)
        {
            dialog_lines.Add(new DialogLine { line = l, enable_typewriter_effect = true });
        }
    }

    void OnEnable()
    {
        if (text == null)
            text = GetComponentInChildren<TMPro.TMP_EmojiTextUGUI>();

        currentLine = -1;
        readTimer = 999;
        clickTimer = 999;

        //have to delay this because the text gets set dynamically AFTER SPAWN, and so i cannot update my text right away
        //Next();

        if (rect == null)
            rect = GetComponentInChildren<RectTransform>();

        if (rect.gameObject != this.gameObject)
            rect.gameObject.SetActive(false);
    }

    void OnDisable()
    {

    }


    // Update is called once per frame
    void Update()
    {
		readTimer += Time.unscaledDeltaTime * readCharsPerSec;

        if (currentLine<0 || readTimer > dialog_lines[currentLine].line.Length)
        {//all shown, wait for click
            clickTimer += Time.unscaledDeltaTime;
            if (autoclickTime > 0
                && clickTimer > autoclickTime)
            {
                Next();
            }
        } else
        {
            var line = dialog_lines[currentLine].line;
            var typing_text = line.Substring(0, Mathf.CeilToInt(readTimer)) + 
                "<color=#00000000>" + line.Substring(Mathf.FloorToInt(readTimer)) + "</color>";   //also render this bit invisibly to keep the formatting the same ;)

            var final_text = dialog_lines[currentLine].enable_typewriter_effect ? typing_text : line;
            text.SetText(final_text);

            clickTimer = 0;
        }
    }


    public void Clear()
    {
        lines.Clear();
        dialog_lines.Clear();
    }

    void Next()
    {
        if (++currentLine >= dialog_lines.Count)
        {
            Clear();

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
            if (rect.gameObject != this.gameObject)
                rect.gameObject.SetActive(true);

            //new: process audio
            //var curr_line = dialog_lines[currentLine].line;
            //if (curr_line.StartsWith("[audio:"))
            //{
            //    curr_line = curr_line.Remove(0, "[audio:".Length);
            //    int e = curr_line.IndexOf("]");
            //    string audioClipName = curr_line.Substring(0, e);
            //    curr_line = curr_line.Substring(audioClipName.Length + 1);
            //
            //    //Debug.Log("audioClipName = " + audioClipName + ", line = " + lines[currentLine]);
            //
            //    SoundManager.GetOrCreateInstance().play_audio(audioClipName);
            //}
            var sections = strip_audio_tag(dialog_lines[currentLine].line);
            dialog_lines[currentLine].line = sections.line;
            if (sections.audioclip_name != null)
            {
                SoundManager.GetOrCreateInstance().play_audio(sections.audioclip_name);
            }

            float charsPerLine = rect.rect.width / text.fontSize;
            float needLines = 1 + (sections.line.Length / (charsPerLine * 0.8f));
            needLines = Mathf.Max(needLines, 2);

            float width = charsPerLine * text.fontSize;
            float height = needLines * text.fontSize * 1.0f * text.lineSpacing;
            rect.sizeDelta = new Vector2(width, height);

            text.text = "";

            //Debug.Log("CPL: " + charsPerLine);

            (string audioclip_name, string line) strip_audio_tag(string line)
            {
                string audioClipName = null;
                if (line.StartsWith("[audio:"))
                {
                    line = line.Remove(0, "[audio:".Length);
                    int e = line.IndexOf("]");
                    audioClipName = line.Substring(0, e);
                    line = line.Substring(audioClipName.Length + 1);

                    //Debug.Log("audioClipName = " + audioClipName + ", line = " + lines[currentLine]);

                    //SoundManager.GetOrCreateInstance().play_audio(audioClipName);
                }

                return (audioClipName, line);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
        var curr_line = dialog_lines[currentLine].line;
        if (readTimer > curr_line.Length)
        {
            Next();
        }
        else
        {
            readTimer = curr_line.Length;
        }
    }

    public void add_dialog_line(string str, bool enable_typingeffect)
    {
        dialog_lines.Add(new DialogLine { line = str, enable_typewriter_effect = enable_typingeffect });
    }
}
