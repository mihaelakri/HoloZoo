using CommMsgs;
using MemoryPack;
using SVSBluetooth;
using UnityEngine;

public class InitializeConnection : MonoBehaviour
{
    private static InitializeConnection Instance;
    private readonly string serverUUID = "d81a5833-37f4-460d-8a9f-347ff95474ad";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        CommConstants.conn_method = PlayerPrefs.GetString("conn_method");
        Debug.Log("conn_method: " + CommConstants.conn_method);

        if (CommConstants.conn_method == "bluetooth")
        {
            BluetoothForAndroid.Initialize();
            EnsureBTEnabled();

            if (PlayerPrefs.GetString("device") == "mobile")
                ServerStart();
            else
                ClientConnect();
        }
        else
            Debug.LogError($"{nameof(InitializeConnection)} - Unknown connection method");
    }

    // Handle Android's activity lifecycle to ensure bluetooth turns off on exit
    void OnApplicationPause(bool pause)
    {
        if (PlayerPrefs.GetString("device") == "mobile")
        {
            if (pause)
            {
                BluetoothForAndroid.StopServer();
                Debug.Log("Bluetooth - onPause, StopServer");
            }
            else
            {
                ServerStart();
                Debug.Log("Bluetooth - onPause, ServerStart");
            }
        }
        else
        {
            if (pause)
            {
                BluetoothForAndroid.Disconnect();
                Debug.Log("Bluetooth - onPause, Disconnect");
            }
            else
            {
                ClientConnect();
                Debug.Log("Bluetooth - onPause, ClientConnect");
            }
        }
    }

    void EnsureBTEnabled()
    {
        if (!BluetoothForAndroid.IsBTEnabled())
        {
            BluetoothForAndroid.EnableBT();
            Debug.Log("Bluetooth - EnabledBT");
        }
    }

    void ServerStart()
    {
        EnsureBTEnabled();
        BluetoothForAndroid.CreateServer(serverUUID);
        Debug.Log("Bluetooth - CreateServer");
    }

    void ClientConnect()
    {
        BluetoothForAndroid.ConnectToServer(serverUUID);
        Debug.Log("Bluetooth - ConnectToServer");
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
        Debug.Log($"Bluetooth - BTDeviceSelected, data: {data}");
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
            CheckInternetConnection.ShowToast("Bluetooth disconnected");
    }
    private void BTFailConnectToServer()
    {
        Debug.Log("Bluetooth - BTFailConnectToServer");
        BTReconnect();  // This loops trying to reconnect
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

    public void EnableBluetoothDiscoverability(int duration = 300)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.bluetooth.adapter.action.REQUEST_DISCOVERABLE");

            intent.Call<AndroidJavaObject>("putExtra", "android.bluetooth.adapter.extra.DISCOVERABLE_DURATION", duration);
            currentActivity.Call("startActivity", intent);
        }
    }
}