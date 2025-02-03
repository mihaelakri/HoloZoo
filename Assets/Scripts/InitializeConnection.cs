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
using SVSBluetooth;
using System.Threading;

public class InitializeConnection : MonoBehaviour
{
    private float lastUpdateTime = 0f;
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

    // Start is called before the first frame update
    async Task Start()
    {
        StartCoroutine(mockAuth());
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        player_id = PlayerPrefs.GetInt("ID");
        CommConstants.conn_method = PlayerPrefs.GetString("conn_method");
        Debug.Log("conn_method: "+CommConstants.conn_method);

        if (CommConstants.conn_method == "bluetooth") {
            BluetoothForAndroid.Initialize();
            if (PlayerPrefs.GetString("device")=="mobile") {
                Debug.Log("Bluetooth - CreateServer");
                BluetoothForAndroid.CreateServer("d81a5833-37f4-460d-8a9f-347ff95474ad");
            } else {
                Debug.Log("Bluetooth - ConnectToServer");
                BluetoothForAndroid.ConnectToServer("d81a5833-37f4-460d-8a9f-347ff95474ad");
            }
        } else {
            Debug.LogError("Unknown connection method");
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
