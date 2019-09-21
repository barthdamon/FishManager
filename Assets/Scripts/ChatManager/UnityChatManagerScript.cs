using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Timers;
using WebSocketSharp;
using System.Text;

using Newtonsoft.Json;

public class UnityChatManagerScript : MonoBehaviour
{
    WebSocket ws;

    int counter = 0;
    Timer ping_timer;

    // test spawn object from chat
    public GameObject spawn_object;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDestroy()
    {
        ping_timer.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(counter > 0)
        {
            Instantiate(spawn_object, new Vector3(0,10,0), Quaternion.identity);
            counter--;
        }
        //Debug.Log("update");
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
            Debug.Log("close!");
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log(e.Data);
            if (e.Data.Contains("shoot"))
            {
                counter++;
                Debug.Log(counter);
                // Now we can actually spawn a bob object
                //Instantiate(box, new Vector3(0,10,0), Quaternion.identity);
            }
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
        m.data = "Unity";
        string json = JsonConvert.SerializeObject(m);
        ws.Send(json);
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
