using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PusherClient;
using System.Threading.Tasks;

public class ConnectionPusher : ConnectionBase
{
    private float updateFrequency = 1f / 9f; // 10Hz is max. allowed by Pusher
    private float lastUpdateTime = 0f;
    protected ConnectionPusher instance = null;

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
        if (CommConstants.pusher == null)
        {
            MyHttpChannelAuthorizer authorizer = new MyHttpChannelAuthorizer(CommConstants.ServerURL+"broadcasting/auth");

            CommConstants.pusher = new Pusher("8264e84d03d49bc6ff4f", new PusherOptions()
            {
                Cluster = "eu",
                Encrypted = true,
                Authorizer = authorizer,
                ClientTimeout = TimeSpan.FromSeconds(20),
            });

            CommConstants.pusher.Error += OnPusherOnError;
            CommConstants.pusher.ConnectionStateChanged += PusherOnConnectionStateChanged;
            CommConstants.pusher.Connected += PusherOnConnected;
            CommConstants.channel = await CommConstants.pusher.SubscribeAsync("private-rotation."+player_id);
            CommConstants.pusher.Subscribed += OnChannelOnSubscribed;
            await CommConstants.pusher.ConnectAsync();
        }
    }

    private void PusherOnConnected(object sender)
    {
        Debug.Log("Pusher - Connected");
        CommConstants.channel.Bind("client-rotation"+player_id, (string data) =>
        {
            Debug.Log("client-rotation"+player_id);
            Debug.Log(data);
            PusherRotationMsg pusherRotationMsg = JsonUtility.FromJson<PusherRotationMsg>(data);
            RotationMsg rotationMsg = JsonUtility.FromJson<RotationMsg>(pusherRotationMsg.data);
            Debug.Log("Rotation Msg; X: "+rotationMsg.x+" Y: "+rotationMsg.y);
            CommConstants.x = float.Parse(rotationMsg.x);
            CommConstants.y = float.Parse(rotationMsg.y);
            CommConstants.z = float.Parse(rotationMsg.z);
            CommConstants.new_animal_id = rotationMsg.animal_id;
        });
        CommConstants.is_websocket_open = true;
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
        if (CommConstants.pusher != null)
        {
            await CommConstants.pusher.DisconnectAsync();
        }
    }


    public override void SendData()
    {
        base.SendData();
        
        if (!CommConstants.is_websocket_open)
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

        RotationMsg rotationMsg = new RotationMsg(
            CommConstants.x.ToString(),
            CommConstants.y.ToString(),
            CommConstants.z.ToString(),
            CommConstants.player_id,
            PlayerPrefs.GetString("id_animal", "1")
        );

        CommConstants.channel.Trigger(
            "client-rotation" + CommConstants.player_id,
            JsonUtility.ToJson(rotationMsg)
        );
        yield return 0;
    }
}
