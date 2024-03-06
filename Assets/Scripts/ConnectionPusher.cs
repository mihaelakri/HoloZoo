using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PusherClient;
using System.Threading.Tasks;
using CommunicationMsgs;

public class ConnectionPusher : ConnectionBase
{
    private float updateFrequency = 1f / 9f; // 10Hz is max. allowed by Pusher
    private float lastUpdateTime = 0f;
    protected ConnectionPusher instance;
    protected Pusher pusher;
    public static Channel channel;
    protected bool is_websocket_open;

    private class PusherRotationMsg {
        public string @event;
        public string data;
        public string channel;
    }

    public override async Task Initialize()
    {
        base.Initialize();

        lastUpdateTime = Time.time;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        await InitializePusher();
    }
    
    private async Task InitializePusher()
    {
        if (pusher == null)
        {
            MyHttpChannelAuthorizer authorizer = new MyHttpChannelAuthorizer(CommConstants.ServerURL+"broadcasting/auth");

            pusher = new Pusher("8264e84d03d49bc6ff4f", new PusherOptions()
            {
                Cluster = "eu",
                Encrypted = true,
                Authorizer = authorizer,
                ClientTimeout = TimeSpan.FromSeconds(20),
            });

            pusher.Error += OnPusherOnError;
            pusher.ConnectionStateChanged += PusherOnConnectionStateChanged;
            pusher.Connected += PusherOnConnected;
            channel = await pusher.SubscribeAsync("private-rotation."+player_id);
            pusher.Subscribed += OnChannelOnSubscribed;
            await pusher.ConnectAsync();
        }
    }

    private void PusherOnConnected(object sender)
    {
        Debug.Log("Pusher - Connected");
        channel.Bind("client-rotation"+player_id, (string data) =>
        {
            Debug.Log("client-rotation"+player_id);
            Debug.Log(data);
            PusherRotationMsg pusherRotationMsg = JsonUtility.FromJson<PusherRotationMsg>(data);
            RotationMsg rotationMsg = JsonUtility.FromJson<RotationMsg>(pusherRotationMsg.data);
            Debug.Log("Rotation Msg; X: "+rotationMsg.x+" Y: "+rotationMsg.y);
            CommConstants.rotationMsg = rotationMsg;
        });
        is_websocket_open = true;
    }

    private void PusherOnConnectionStateChanged(object sender, ConnectionState state)
    {
        Debug.Log("Pusher - Connection state changed");
    }

    private void OnPusherOnError(object s, PusherException e)
    {
        Debug.Log("Pusher - Errored");
        Debug.Log(e);
    }

    private void OnChannelOnSubscribed(object s, Channel c)
    {
        Debug.Log("Pusher - Subscribed");
    }

    async Task OnApplicationQuit()
    {
        if (pusher != null)
        {
            await pusher.DisconnectAsync();
        }
    }


    public override void SendData()
    {
        base.SendData();
        
        if (!is_websocket_open)
        {
            Debug.Log("Websocket not open");
            return;
        }

        float timeSinceLastUpdate = Time.time - lastUpdateTime;

        if (timeSinceLastUpdate >= updateFrequency)
        {
            StartCoroutine(SendOverNetwork());
            lastUpdateTime = Time.time;
        }
    }

    IEnumerator SendOverNetwork()
    {
        // Debug.Log(PlayerPrefs.GetString("id_animal"));

        channel.Trigger(
            "client-rotation" + CommConstants.rotationMsg.player_id,
            JsonUtility.ToJson(CommConstants.rotationMsg)
        );
        yield return 0;
    }
}
