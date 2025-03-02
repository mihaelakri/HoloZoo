using CommMsgs;
using MemoryPack;
using SVSBluetooth;
using UnityEngine;

public class InitializeConnection : MonoBehaviour
{
    private static InitializeConnection Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CommConstants.conn_method = PlayerPrefs.GetString("conn_method");
        Debug.Log("conn_method: " + CommConstants.conn_method);

        if (CommConstants.conn_method == "bluetooth")
        {
            BluetoothForAndroid.Initialize();
            if (PlayerPrefs.GetString("device") == "mobile")
            {
                Debug.Log("Bluetooth - CreateServer");
                BluetoothForAndroid.CreateServer("d81a5833-37f4-460d-8a9f-347ff95474ad");
            }
            else
            {
                Debug.Log("Bluetooth - ConnectToServer");
                BluetoothForAndroid.ConnectToServer("d81a5833-37f4-460d-8a9f-347ff95474ad");
            }
        }
        else
        {
            Debug.LogError("Unknown connection method");
        }
    }

    // Bluetooth events
    private void OnEnable()
    {
        BluetoothForAndroid.DeviceConnected += BTConnected;
        BluetoothForAndroid.DeviceDisconnected += BTDisconnected;
        BluetoothForAndroid.DeviceSelected += BTDeviceSelected;
        if (PlayerPrefs.GetString("device") == "tablet")
        {
            BluetoothForAndroid.ReceivedByteMessage += BTReceiveRotate3DModel;
            BluetoothForAndroid.FailConnectToServer += BTFailConnectToServer;
        }
    }
    private void OnDisable()
    {
        BluetoothForAndroid.DeviceConnected -= BTConnected;
        BluetoothForAndroid.DeviceDisconnected -= BTDisconnected;
        BluetoothForAndroid.DeviceSelected -= BTDeviceSelected;
        if (PlayerPrefs.GetString("device") == "tablet")
        {
            BluetoothForAndroid.ReceivedByteMessage -= BTReceiveRotate3DModel;
            BluetoothForAndroid.FailConnectToServer -= BTFailConnectToServer;
        }
    }
    private void BTDeviceSelected(string data)
    {
        Debug.Log("Bluetooth - BTDeviceSelected");
        Debug.Log("Bluetooth - Data: " + data);
        CommConstants.paired_BT_server = data.Split(',')[1];
    }
    private void BTConnected()
    {
        Debug.Log("Bluetooth - BTConnected");
        CommConstants.is_BTConnected = true;
        CheckInternetConnection.ShowToast("Bluetooth connected");
    }
    private void BTDisconnected()
    {
        Debug.Log("Bluetooth - BTDisconnected");
        CommConstants.is_BTConnected = false;
        if (PlayerPrefs.GetString("device") == "tablet")
        {
            CheckInternetConnection.ShowToast("Bluetooth disconnected, reconnecting...");
            BTReconnect();
        }
        else
        {
            CheckInternetConnection.ShowToast("Bluetooth disconnected");
        }
    }
    private void BTFailConnectToServer()
    {
        Debug.Log("Bluetooth - BTFailConnectToServer");
        BTReconnect();
    }
    private void BTReconnect()
    {
        Debug.Log("Bluetooth - BTReconnect");
        BluetoothForAndroid.ConnectToServerByAddress("d81a5833-37f4-460d-8a9f-347ff95474ad", CommConstants.paired_BT_server);
    }
    private void BTReceiveRotate3DModel(byte[] data)
    {
        RotationMsg rotationMsg = MemoryPackSerializer.Deserialize<RotationMsg>(data);

        CommConstants.x = rotationMsg.x;
        CommConstants.y = rotationMsg.y;
        CommConstants.z = rotationMsg.z;
        CommConstants.animal_id = rotationMsg.animal_id;
        // Debug.Log("Bluetooth - BTReceiveRotate3DModel: " + CommConstants.x + ", " + CommConstants.y + ", " + CommConstants.z + ", " + CommConstants.animal_id);
    }
}