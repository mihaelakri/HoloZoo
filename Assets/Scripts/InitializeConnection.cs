using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using PusherClient;
using SVSBluetooth;
using System.Threading;

public class InitializeConnection : MonoBehaviour
{
    private float updateFrequency = 1f / 9f; // 10Hz is max. allowed by Pusher
    private float lastUpdateTime = 0f;
    // private Pusher pusher;
    // private Channel channel;
    private int player_id;
    Scene m_Scene;
    string sceneName;
    public InitializeConnection instance = null;
    private class RotationMsg {
        public string x;
        public string y;
        public string z;
        public int player_id;
        public string animal_id;

        public RotationMsg(string x, string y, string z, int player_id, string animal_id){
            this.x = x;
            this.y = y;
            this.z = z;
            this.player_id = player_id;
            this.animal_id = animal_id;
        }
    }

    private class PusherRotationMsg {
        public string @event;
        public string data;
        public string channel;
    }

    // Start is called before the first frame update
    async Task Start()
    {
        StartCoroutine(mockAuth());
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        player_id = PlayerPrefs.GetInt("ID");
        CommConstants.conn_method = PlayerPrefs.GetString("conn_method");
        Debug.Log("conn_method: "+CommConstants.conn_method);

        if (CommConstants.conn_method == "websocket") {
            lastUpdateTime = Time.time;
            // Debug.Log("Player id: "+player_id);
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            await InitialisePusher();
        } else {
            BluetoothForAndroid.Initialize();
            if (PlayerPrefs.GetString("device")=="mobile") {
                Debug.Log("Bluetooth - CreateServer");
                BluetoothForAndroid.CreateServer("d81a5833-37f4-460d-8a9f-347ff95474ad");
            } else {
                Debug.Log("Bluetooth - ConnectToServer");
                BluetoothForAndroid.ConnectToServer("d81a5833-37f4-460d-8a9f-347ff95474ad");
            }
        }
    }

    // Websockets

    private async Task InitialisePusher()
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

    // Bluetooth

    private void OnEnable () {
        BluetoothForAndroid.ReceivedStringMessage += BTReceiveRotate3DModel;
        if (PlayerPrefs.GetString("device")!="mobile") {
            BluetoothForAndroid.FailConnectToServer += BTReconnect;
            BluetoothForAndroid.DeviceDisconnected += BTReconnect;
            BluetoothForAndroid.DeviceConnected += BTConnected;
            BluetoothForAndroid.DeviceDisconnected += BTDisconnected;
            BluetoothForAndroid.DeviceSelected += BTDeviceSelected;
        }
    }
    private void OnDisable () {
        if (PlayerPrefs.GetString("device")!="mobile") {
            BluetoothForAndroid.ReceivedStringMessage -= BTReceiveRotate3DModel;
            BluetoothForAndroid.FailConnectToServer -= BTReconnect;
            BluetoothForAndroid.DeviceDisconnected -= BTReconnect;
            BluetoothForAndroid.DeviceConnected -= BTConnected;
            BluetoothForAndroid.DeviceDisconnected -= BTDisconnected;
            BluetoothForAndroid.DeviceSelected -= BTDeviceSelected;
        }
    }
    private void BTDeviceSelected(string data) {
        Debug.Log("Bluetooth - BTDeviceSelected");
        Debug.Log("Bluetooth - Data: " + data);
        CommConstants.paired_BT_server = data.Split(',')[1];
    }
    private void BTConnected(){
        Debug.Log("Bluetooth - BTConnected");
        CommConstants.is_BTConnected = true;
    }
    private void BTDisconnected(){
        Debug.Log("Bluetooth - BTDisconnected");
        CommConstants.is_BTConnected = false;
        BTReconnect();
    }
    private void BTReconnect(){
        Debug.Log("Bluetooth - BTReconnect");
        BluetoothForAndroid.ConnectToServerByAddress("d81a5833-37f4-460d-8a9f-347ff95474ad", CommConstants.paired_BT_server);
    }

    private void BTReceiveRotate3DModel (string data) {
        Debug.Log("Bluetooth - BTReceiveRotate3DModel");
        RotationMsg rotationMsg = JsonUtility.FromJson<RotationMsg>(data);
        // Quaternion localRotation = Quaternion.Euler(float.Parse(rotationMsg.x), float.Parse(rotationMsg.y), 0f);
        // model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;
        CommConstants.x = float.Parse(rotationMsg.x);
        CommConstants.y = float.Parse(rotationMsg.y);
        CommConstants.z = float.Parse(rotationMsg.z);
        CommConstants.new_animal_id = rotationMsg.animal_id;
    }


    IEnumerator mockAuth(){
        string id_animal = "0";

        using (UnityWebRequest www = UnityWebRequest.Get(CommConstants.ServerURL+"animal/model/"+id_animal)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    Debug.Log(www.downloadHandler.text);

                    // foreach (var s in www.GetResponseHeaders()) {
                    //     Debug.Log("s=" + s);
                    // }
                    foreach (var s in www.GetResponseHeader("Set-Cookie").Split(';')) {
                        if(s.Contains("holozoo_session")){
                            CommConstants.Auth = s.Substring(s.IndexOf("holozoo_session")).Split('=')[1].Split(';')[0];
                            Debug.Log(CommConstants.Auth);

                        } else if (s.Contains("XSRF-TOKEN")){
                            CommConstants.XSRF = s.Substring(s.IndexOf("XSRF-TOKEN")).Split('=')[1].Split(';')[0];
                            Debug.Log(CommConstants.XSRF);

                        }
                    }
                    // Debug.Log(www.GetResponseHeader("Set-Cookie"));
                    // CommConstant.Auth = www.GetRequestHeader("Cookie");
                }
        }
    }
}
