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

public class RotateModel : MonoBehaviour
{

    public Slider bottomSlider;
    public Slider sideSlider;
    public GameObject model;
    public GameObject target;
    public float x,y;
    public float sliderLastX=0, sliderLastY=0;

    private float updateFrequency = 1f / 9f; // 10Hz is max. allowed by Pusher
    private float lastUpdateTime = 0f;
    public RotateModel instance = null;
    private Pusher pusher;
    private Channel channel;
    private bool is_websocket_open = false;
    private int player_id;
    private string conn_method;
    private class RotationMsg {
        public string x;
        public string y;
        public int player_id;

        public RotationMsg(string x, string y, int player_id){
            this.x = x;
            this.y = y;
            this.player_id = player_id;
        }
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

    // Websockets

    private async Task InitialisePusher()
    {
        if (pusher == null)
        {
            MyHttpChannelAuthorizer authorizer = new MyHttpChannelAuthorizer(CommConstants.ServerURL+"broadcasting/auth");
            // {
            //     AuthenticationHeader = new AuthenticationHeaderValue("Cookie", "holozoo_session=eyJpdiI6IkE1eDZNOWg5cFMzR29aNnZNeDhxd2c9PSIsInZhbHVlIjoieTNrcnJzajNOL1ZIaXVwQ1MyUlNFZUNGSk9Od3Zoc0UzUy9TL1l5K2d5TFdJaVcwWDVDOGZGYXdOM2NDdUI0R3J3aDRLUUkrdFRBRDhyT1FLYzE4U0JMdURacnRMeHlZdEpGMlg2amp5Q3ZteTNRZVNBSlpidDZsVm56MTl0NDQiLCJtYWMiOiI3ODU2ZDgzYjkxMzI0MjYzODc4YmUzZDljMGJlNzlkMDJiYTk4ZTA4ZmIxZGJiODQyMDM0N2YwZTU1Y2YyZjVhIiwidGFnIjoiIn0%3D"),
            // };
            // Dictionary<string, string> headers = new Dictionary<string, string>();

            // If using session-based auth, add session cookie to headers
            // headers.Add("Cookie", "laravel_session=eyJpdiI6IkE1eDZNOWg5cFMzR29aNnZNeDhxd2c9PSIsInZhbHVlIjoieTNrcnJzajNOL1ZIaXVwQ1MyUlNFZUNGSk9Od3Zoc0UzUy9TL1l5K2d5TFdJaVcwWDVDOGZGYXdOM2NDdUI0R3J3aDRLUUkrdFRBRDhyT1FLYzE4U0JMdURacnRMeHlZdEpGMlg2amp5Q3ZteTNRZVNBSlpidDZsVm56MTl0NDQiLCJtYWMiOiI3ODU2ZDgzYjkxMzI0MjYzODc4YmUzZDljMGJlNzlkMDJiYTk4ZTA4ZmIxZGJiODQyMDM0N2YwZTU1Y2YyZjVhIiwidGFnIjoiIn0%3D");
            // headers.Add("Cookie", "XSRF-TOKEN=eyJpdiI6IkxEUlQ2SEZCK0QxUnhLTFpiRjJrU1E9PSIsInZhbHVlIjoieXNvNXp1UjJFeFB1bEE4a0o5RDlwOXlVTzRHa2RMNmwwUFlENGZjMHNBeTdvS2ZRbm5xMGZBeU5yM1ladjlWaXRNUncrUlpFR1VyT3dHUmg0TE5JSFJsU1ZGZFF4OWNlbzh5T1lLS2MzVkJRditNVVg1KzVGL0M3SmE2cUg1MGQiLCJtYWMiOiI4NjliNDJkNWE0ZDZkN2E5MDMzZjhiYTg1Y2IwNWZmODYxOWFhZmYwODc5NjdjN2IwZTk3ZmUzM2Q3ZGVmNWM1IiwidGFnIjoiIn0%3D");

            // authorizer.setAuthenticationHeader(headers);

            pusher = new Pusher("8264e84d03d49bc6ff4f", new PusherOptions()
            {
                Cluster = "eu",
                Encrypted = true,
                // Authorizer = new HttpAuthorizer(CommConstants.ServerURL+"broadcasting/auth")
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
        Quaternion localRotation = Quaternion.Euler(sideSlider.value,bottomSlider.value, 0f);
        model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;

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
            player_id
        );

        channel.Trigger(
            "client-rotation"+player_id,
            JsonUtility.ToJson(rotationMsg)
        );
        yield return 0;

        // WWWForm form = new WWWForm();
        // form.AddField("x", sideSlider.value.ToString());
        // form.AddField("y", bottomSlider.value.ToString());
        // form.AddField("id", PlayerPrefs.GetInt("ID"));

        // using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"rotate", form)){

        //     yield return www.SendWebRequest();

        //     if (www.result != UnityWebRequest.Result.Success)
        //         {
        //             Debug.Log(www.error);
        //         }
        //     else
        //         {
        //              Debug.Log(www.downloadHandler.text);
        //         }
        // }
    }

    private void PusherOnConnected(object sender)
    {
        Debug.Log("Pusher - Connected");
        channel.Bind("client-rotation"+player_id, (string data) =>
        {
            Debug.Log("client-rotation"+player_id);
            Debug.Log(data);
            // StartCoroutine(ReceiveRotate3DModel(JsonUtility.FromJson<RotationMsg>(data)));
            RotationMsg rotationMsg = JsonUtility.FromJson<RotationMsg>(data);
            Quaternion localRotation = Quaternion.Euler(float.Parse(rotationMsg.x), float.Parse(rotationMsg.y), 0f);
            model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;
        });
        is_websocket_open = true;
    }
    // IEnumerator ReceiveRotate3DModel(RotationMsg rotationMsg){
    //     Quaternion localRotation = Quaternion.Euler(rotationMsg.x, rotationMsg.y, 0f);
    //     model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;

    //     yield return 0;

    // }
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
    }
    private void OnDisable () {
        BluetoothForAndroid.ReceivedStringMessage -= BTReceiveRotate3DModel;
    }

    private void BTSendRotate3DModel () {
        Debug.Log("Bluetooth - BTSendRotate3DModel");
        RotationMsg rotationMsg = new RotationMsg(
            sideSlider.value.ToString(),
            bottomSlider.value.ToString(),
            player_id
        );
        BluetoothForAndroid.WriteMessage(JsonUtility.ToJson(rotationMsg));
    }

    private void BTReceiveRotate3DModel (string data) {
        Debug.Log("Bluetooth - BTReceiveRotate3DModel");
        RotationMsg rotationMsg = JsonUtility.FromJson<RotationMsg>(data);
        Quaternion localRotation = Quaternion.Euler(float.Parse(rotationMsg.x), float.Parse(rotationMsg.y), 0f);
        model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;
    }
}


