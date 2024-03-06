using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVSBluetooth;
using System.Threading.Tasks;
using CommunicationMsgs;

public class ConnectionBluetooth : ConnectionBase
{
    protected bool is_BTConnected = false;
    protected string paired_BT_server;
    public override async Task Initialize()
    {
        base.Initialize();
        
        BluetoothForAndroid.Initialize();
        if (PlayerPrefs.GetString("device")=="mobile") {
            Debug.Log("Bluetooth - CreateServer");
            BluetoothForAndroid.CreateServer("d81a5833-37f4-460d-8a9f-347ff95474ad");
        } else {
            Debug.Log("Bluetooth - ConnectToServer");
            BluetoothForAndroid.ConnectToServer("d81a5833-37f4-460d-8a9f-347ff95474ad");
        }
    }

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
        paired_BT_server = data.Split(',')[1];
    }
    private void BTConnected(){
        Debug.Log("Bluetooth - BTConnected");
        is_BTConnected = true;
    }
    private void BTDisconnected(){
        Debug.Log("Bluetooth - BTDisconnected");
        is_BTConnected = false;

        BTReconnect();
    }
    private void BTReconnect(){
        Debug.Log("Bluetooth - BTReconnect");
        BluetoothForAndroid.ConnectToServerByAddress("d81a5833-37f4-460d-8a9f-347ff95474ad", paired_BT_server);
    }

    private void BTReceiveRotate3DModel (string data) {
        Debug.Log("Bluetooth - BTReceiveRotate3DModel");
        RotationMsg rotationMsg = JsonUtility.FromJson<RotationMsg>(data);

        CommConstants.x = float.Parse(rotationMsg.x);
        CommConstants.y = float.Parse(rotationMsg.y);
        CommConstants.z = float.Parse(rotationMsg.z);
        CommConstants.new_animal_id = rotationMsg.animal_id;
    }
    
    public override void SendData()
    {
        base.SendData();
        
        // Debug.Log("Bluetooth - BTSendRotate3DModel");
        RotationMsg rotationMsg = new RotationMsg(
            CommConstants.x.ToString(),
            CommConstants.y.ToString(),
            CommConstants.z.ToString(),
            CommConstants.player_id,
            PlayerPrefs.GetString("id_animal", "1")
        );
        BluetoothForAndroid.WriteMessage(JsonUtility.ToJson(rotationMsg));
    }

}
