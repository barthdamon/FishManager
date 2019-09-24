using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ChatManagerUIScript : MonoBehaviour
{
    public Button connect_button;
    public Button disconnect_button;
    public Button testsend_button;

    // Start is called before the first frame update
    void Start()
    {
        var connect_btn = connect_button.GetComponent<Button>();
        connect_btn.onClick.AddListener(TaskOnClickConnect);
        var disconnect_btn = disconnect_button.GetComponent<Button>();
        disconnect_btn.onClick.AddListener(TaskOnClickDisconnect);

        var testsend_btn = testsend_button.GetComponent<Button>();
        testsend_btn.onClick.AddListener(TaskOnClickTestSendMsg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TaskOnClickConnect()
    {
        var chat_mgr = UnityChatManagerScript.GetOrCreateInstance();
        if (chat_mgr)
        {
            chat_mgr.ConnectToChat();
        }
    }

    void TaskOnClickDisconnect()
    {
        var chat_mgr = UnityChatManagerScript.GetOrCreateInstance();
        if (chat_mgr)
        {
            chat_mgr.DisconnectFromChat();
        }
    }

    void TaskOnClickTestSendMsg()
    {
        var chat_mgr = GameObject.Find("ChatManager").GetComponent<UnityChatManagerScript>();
        if (chat_mgr)
        {
            chat_mgr.SendChatMessage("Hi Everyone!");
        }
    }
}
