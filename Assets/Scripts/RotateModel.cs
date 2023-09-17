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

public class RotateModel : MonoBehaviour
{

    public Slider bottomSlider;
    public Slider sideSlider;
    public GameObject model;
    public GameObject target;
    public float x=0,y=0;
    public float sliderLastX=0, sliderLastY=0;

    private float updateFrequency = 1f / 9f; // 10Hz is max. allowed by Pusher
    private float lastUpdateTime = 0f;
    public RotateModel instance = null;
    private Pusher pusher;
    private Channel channel;
    private bool is_websocket_open = false;
    private int player_id;
    private string conn_method;
    bool is_BTConnected = false;
    private string new_animal_id;
    private string old_animal_id;
    private string paired_BT_server;
    private class RotationMsg {
        public string x;
        public string y;
        public int player_id;
        public string animal_id;

        public RotationMsg(string x, string y, int player_id, string animal_id){
            this.x = x;
            this.y = y;
            this.player_id = player_id;
            this.animal_id = animal_id;
        }
    }

    private class PusherRotationMsg {
        public string @event;
        public string data;
        public string channel;
    }

    async Task Start()
    {
        player_id = PlayerPrefs.GetInt("ID");

        conn_method = PlayerPrefs.GetString("conn_method");
        Debug.Log("conn_method: "+conn_method);

        if (conn_method == "websocket") {
            lastUpdateTime = Time.time;
            Debug.Log("Player id: "+player_id);
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
    IEnumerator SwapModel() {
        GameObject parent = GameObject.FindGameObjectWithTag("3d");
        Destroy(parent.transform.GetChild(0).gameObject);

        string id_animal = this.new_animal_id;
        // id_animal = "2";

        using (UnityWebRequest www = UnityWebRequest.Get(CommConstants.ServerURL+"animal/model/"+id_animal)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    string model_url = (www.downloadHandler.text);
                    Debug.Log(www.downloadHandler.text);
                    GameObject variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
                    Instantiate(variableForPrefab, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d").transform);
                }
        }

    }
    void Update() 
    {
        if (this.old_animal_id != this.new_animal_id) {
            StartCoroutine(SwapModel());
            this.old_animal_id = this.new_animal_id;
        }

        try {
            Quaternion localRotation = Quaternion.Euler(this.x, this.y, 0f);
            model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;
        } catch (Exception e) {
            Debug.Log(e);
        }
    }

    // Websockets

    private async Task InitialisePusher()
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

    public void rotateModel(){
        this.x = sideSlider.value;
        this.y = bottomSlider.value;
        // Quaternion localRotation = Quaternion.Euler(sideSlider.value,bottomSlider.value, 0f);
        // model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;

        if (conn_method == "websocket") {
            if (!is_websocket_open)
                return;

            float timeSinceLastUpdate = Time.time - lastUpdateTime;

            if (timeSinceLastUpdate >= updateFrequency)
            {
                StartCoroutine(SendRotate3DModel());
                lastUpdateTime = Time.time;
            }
        } else {    // Bluetooth
            BTSendRotate3DModel();
        }

    }

    IEnumerator SendRotate3DModel(){
        // Debug.Log(PlayerPrefs.GetString("id_animal"));

        RotationMsg rotationMsg = new RotationMsg(
            sideSlider.value.ToString(),
            bottomSlider.value.ToString(),
            player_id,
            PlayerPrefs.GetString("id_animal", "1")
        );

        channel.Trigger(
            "client-rotation"+player_id,
            JsonUtility.ToJson(rotationMsg)
        );
        yield return 0;
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
            this.x = float.Parse(rotationMsg.x);
            this.y = float.Parse(rotationMsg.y);
            this.new_animal_id = rotationMsg.animal_id;
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
        this.paired_BT_server = data.Split(',')[1];
    }
    private void BTConnected(){
        Debug.Log("Bluetooth - BTConnected");
        this.is_BTConnected = true;
    }
    private void BTDisconnected(){
        Debug.Log("Bluetooth - BTDisconnected");
        this.is_BTConnected = false;
        BTReconnect();
    }

    private void BTReconnect(){
        Debug.Log("Bluetooth - BTReconnect");
        BluetoothForAndroid.ConnectToServerByAddress("d81a5833-37f4-460d-8a9f-347ff95474ad", this.paired_BT_server);
    }

    // private void BTKeepReconnecting(){
    //     Debug.Log("Bluetooth - BTKeepReconnecting");
    //     while (!this.is_BTConnected) {
    //         BTReconnect();
    //         Thread.Sleep(2000);
    //     }
    // }
    private void BTSendRotate3DModel () {
        Debug.Log("Bluetooth - BTSendRotate3DModel");
        RotationMsg rotationMsg = new RotationMsg(
            sideSlider.value.ToString(),
            bottomSlider.value.ToString(),
            player_id,
            PlayerPrefs.GetString("id_animal", "1")
        );
        BluetoothForAndroid.WriteMessage(JsonUtility.ToJson(rotationMsg));
    }

    private void BTReceiveRotate3DModel (string data) {
        Debug.Log("Bluetooth - BTReceiveRotate3DModel");
        RotationMsg rotationMsg = JsonUtility.FromJson<RotationMsg>(data);
        // Quaternion localRotation = Quaternion.Euler(float.Parse(rotationMsg.x), float.Parse(rotationMsg.y), 0f);
        // model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;
        this.x = float.Parse(rotationMsg.x);
        this.y = float.Parse(rotationMsg.y);
        this.new_animal_id = rotationMsg.animal_id;
    }
}


