using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Timers;
using WebSocketSharp;
using System.Text;
using System.Linq;

using Newtonsoft.Json;

public class UnityChatManagerScript : MonoBehaviourSingleton<UnityChatManagerScript>
{
    WebSocket ws;

    //int counter = 0;
    Timer ping_timer;

    public Text chatroom_promotion_text;

    Queue<string> buffer = new Queue<string>();

    public delegate void BufferItemEvent(string username, string message);
    public event BufferItemEvent OnMessage;

    public delegate void UserEvent(string username);
    public event UserEvent OnLogInMessage;
    public event UserEvent OnLogOutMessage;

    string myname = "Unity";
    string logged_in_tok = " logged in.";
    string logged_out_tok = " logged out.";

    // test spawn object from chat
    //public GameObject spawn_object;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDestroy()
    {
        DisconnectFromChat();
    }

    // Update is called once per frame
    void Update()
    {
        //if (UnityEngine.Random.value < 0.01f)
        //{
        //    DialogManager.Get().Say_2D("...", "speaker " + UnityEngine.Random.Range(1, 100));
        //}

        //if(counter > 0)
        //{
        //    Instantiate(spawn_object, new Vector3(0,10,0), Quaternion.identity);
        //    counter--;
        //}
        //Debug.Log("update");

        while(buffer.Count > 0)
        {
            var data = buffer.Dequeue();

            if (data.EndsWith(logged_in_tok) && !data.Contains(":"))
            {
                var username = data.Substring(0, data.Length - logged_in_tok.Length);
                OnLogInMessage?.Invoke(username.Trim());

                if (username.ToLower() == ("@" + myname.ToLower()))
                {
                    chatroom_promotion_text.enabled = true;
                }
            }
            else if (data.EndsWith(logged_out_tok) && !data.Contains(":"))
            {
                var username = data.Substring(0, data.Length - logged_in_tok.Length);
                OnLogOutMessage?.Invoke(username.Trim());

                if (username.ToLower() == ("@" + myname.ToLower()))
                {
                    chatroom_promotion_text.enabled = false;
                }
            }
            else
            {
                var tok = ": ";
                var index = data.IndexOf(tok);
                var username = data.Substring(0, index);
                var message = data.Substring(index + tok.Length); 
                OnMessage?.Invoke(username, message);

                var go = GameObject.Find("TestEmojiText");
                if(go)
                {
                    go.GetComponent<TMPro.TMP_EmojiTextUGUI>().SetText(message);
                }
            }
        }
    }

    struct Message
    {
        public string type;
        public string data;
    }

    public void ConnectToChat()
    {
        if (ws != null) return;

        //var ws = new WebSocket("ws://echo.websocket.org");
        //ws = new WebSocket("ws://localhost:3000/");
        ws = new WebSocket("ws://millionscales.herokuapp.com");

        ws.OnOpen += (sender, e) => {
            Debug.Log("Unity connected successfully");
        };

        ws.OnError += (sender, e) =>
        {
            Debug.Log("error!");
            Debug.Log(e.Message);
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("ws close!");
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log(e.Data);

            buffer.Enqueue(e.Data);
            //DialogManager.Get().Buffer(e.Data);
			//FMCodfather.GetOrCreateInstance().Buffer(e.Data);
			/*
            if (e.Data.Contains("shoot"))
            {
                counter++;
                Debug.Log(counter);
                // Now we can actually spawn a bob object
                //Instantiate(box, new Vector3(0,10,0), Quaternion.identity);
            }*/
		};

        ws.Connect();

        // timer to ping the server every 10 sec so we keep the connection alive
        ping_timer = new Timer();
        ping_timer.Interval = 10000;
        ping_timer.Elapsed += (sender, e) =>
        {
            //Debug.Log("Ping");
            ws.Ping("Ping msg");
        };
        ping_timer.Start();

        Message m;
        m.type = "login";
        m.data = myname;
        string json = JsonConvert.SerializeObject(m);
        ws.Send(json);
    }

    public void DisconnectFromChat()
    {
        if(ws != null)
        {
            // stop ping timer
            if (ping_timer != null)
            {
                ping_timer.Stop();
            }

            // remove Avatar
            string logout_str = "@" + myname + logged_out_tok;
            buffer.Enqueue(logout_str);
            chatroom_promotion_text.enabled = false;

            // close and delete ws
            ws.Close();
            ws = null;
        }
    }
    public void SendChatMessage(string s)
    {
        if (ws == null) return;

        Debug.Log("Unity try sending: " + s);
        Message m;
        m.type = "message";
        m.data = s;
        string json = JsonConvert.SerializeObject(m);
        ws.Send(json);
    }
}
