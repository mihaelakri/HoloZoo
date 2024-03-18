#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVSBluetooth;
using CommunicationMsgs;
using Newtonsoft.Json;

public class ConnectionBluetooth : ConnectionBase
{
    private float updateFrequency = 1f/20f; 
    private float lastUpdateTime = 0f;
    protected bool is_BTConnected = false;
    protected string paired_BT_server;
    public override void Initialize()
    {
        base.Initialize();
        
        BluetoothForAndroid.Initialize();
        lastUpdateTime = Time.time;
        Debug.Log("Bluetooth - ConnectToServer");
        BluetoothForAndroid.ConnectToServer("d81a5833-37f4-460d-8a9f-347ff95474ad");
    }

    private void OnEnable () {
        BluetoothForAndroid.ReceivedStringMessage += BTReceiveRotate3DModel;
        BluetoothForAndroid.FailConnectToServer += BTReconnect;
        BluetoothForAndroid.DeviceDisconnected += BTReconnect;
        BluetoothForAndroid.DeviceConnected += BTConnected;
        BluetoothForAndroid.DeviceDisconnected += BTDisconnected;
        BluetoothForAndroid.DeviceSelected += BTDeviceSelected;
    }
    private void OnDisable () {
        BluetoothForAndroid.ReceivedStringMessage -= BTReceiveRotate3DModel;
        BluetoothForAndroid.FailConnectToServer -= BTReconnect;
        BluetoothForAndroid.DeviceDisconnected -= BTReconnect;
        BluetoothForAndroid.DeviceConnected -= BTConnected;
        BluetoothForAndroid.DeviceDisconnected -= BTDisconnected;
        BluetoothForAndroid.DeviceSelected -= BTDeviceSelected;
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
        Debug.Log("Data: " + data);
        //StateMsg stateMsg = JsonUtility.FromJson<StateMsg>(data);
        // StateMsg stateMsg = JsonConvert.DeserializeObject<StateMsg>(data);

        // CommConstants.state = stateMsg;
        base.ParseAndStoreMsg(data);
    }
    
    public override void SendData(CommunicationMsgs.CommunicationMsg msgType)
    {
        float timeSinceLastUpdate = Time.time - lastUpdateTime;

        if (timeSinceLastUpdate >= updateFrequency)
        {
            base.SendData(msgType);
            lastUpdateTime = Time.time;

            //string msg=JsonUtility.ToJson(CommConstants.state);
            // string msg = JsonConvert.SerializeObject(base.msg, Formatting.None);
            //BluetoothForAndroid.WriteMessage(JsonUtility.ToJson(CommConstants.state));
            BluetoothForAndroid.WriteMessage(base.msg);
            Debug.Log("Bluetooth - Data Sent: " + base.msg);
        }  
    }

}
#endif