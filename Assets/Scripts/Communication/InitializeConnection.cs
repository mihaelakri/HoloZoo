using System.Linq;
using CommMsgs;
using MemoryPack;
using SVSBluetooth;
using UnityEngine;

public class InitializeConnection : MonoBehaviour
{
    public static InitializeConnection Instance { get; private set; }
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
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(Start)}, device: {PlayerPrefs.GetString("device")}");

        BluetoothForAndroid.Initialize();
        EnsureBTEnabled();

        if (PlayerPrefs.GetString("device") == "mobile")
            ServerStart();
        else
            ClientConnect(false);
    }

    void EnsureBTEnabled()
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(EnsureBTEnabled)}");

        if (!BluetoothForAndroid.IsBTEnabled())
            BluetoothForAndroid.EnableBT();
    }

    public void ServerStart()
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(ServerStart)}, serverUUID: {serverUUID}");

        EnsureBTEnabled();
        BluetoothForAndroid.StopServer();
        BluetoothForAndroid.CreateServer(serverUUID);
    }

    public void ServerStop()
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(ServerStop)}");

        BluetoothForAndroid.StopServer();
    }

    public bool IsConnected()
    {
        string connectedDevice = BluetoothForAndroid.GetConnectedDeviceName();
        bool isConnected = connectedDevice != "not connected";
        bool hasName = connectedDevice != "no name";

        return isConnected && hasName;
    }

    public bool IsPaired()
    {
        string deviceAddress = PlayerPrefs.GetString("BTDevAddress", "not set");
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(IsPaired)}, deviceAddress: {deviceAddress}");
        return BluetoothForAndroid.GetBondedDevices().Where(dev => dev.address == deviceAddress).Any();
    }

    public void ClientConnect(bool forced = false)
    {
        bool isReallyPaired = IsPaired();
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(ClientConnect)}, isReallyPaired: {isReallyPaired}");

        if (isReallyPaired)
            ConnectExistingDevice();
        else if (forced)
            ConnectNewDevice();
    }

    void ConnectExistingDevice()
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(ConnectExistingDevice)}");
        BTReconnect();
    }

    void ConnectNewDevice()
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(ConnectNewDevice)}");
        ForgetDevice();
        BluetoothForAndroid.ConnectToServer(serverUUID);
    }

    public void Disconnect()
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(Disconnect)}");

        BluetoothForAndroid.Disconnect();
    }

    public void ForgetDevice()
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(ForgetDevice)}");

        PlayerPrefs.DeleteKey("BTDevName");
        PlayerPrefs.DeleteKey("BTDevAddress");
        PlayerPrefs.Save();

        Disconnect();
    }

    public void EnableBluetoothDiscoverability(int duration = 300)
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(EnableBluetoothDiscoverability)}, for {duration} seconds");

        if (Application.platform == RuntimePlatform.Android)
        {
            using AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.bluetooth.adapter.action.REQUEST_DISCOVERABLE");

            intent.Call<AndroidJavaObject>("putExtra", "android.bluetooth.adapter.extra.DISCOVERABLE_DURATION", duration);
            currentActivity.Call("startActivity", intent);
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
    private void BTDeviceSelected(string deviceNameAddress)
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(BTDeviceSelected)}, (Name,Address): {deviceNameAddress}");

        PlayerPrefs.SetString("BTDevName", deviceNameAddress.Split(',')[0]);
        PlayerPrefs.SetString("BTDevAddress", deviceNameAddress.Split(',')[1]);
    }
    private void BTConnected()
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(BTConnected)}");

        // Refresh name in case user changed it
        PlayerPrefs.SetString("BTDevName", BluetoothForAndroid.GetConnectedDeviceName());
        CheckInternetConnection.ShowToast(GameData.Instance.translations.bluetoothToasts.connected);
    }
    private void BTDisconnected()
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(BTDisconnected)}");

        CheckInternetConnection.ShowToast(GameData.Instance.translations.bluetoothToasts.disconnected);
        if (PlayerPrefs.GetString("device") == "mobile")
            ServerStart();
    }
    private void BTFailConnectToServer()
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(BTFailConnectToServer)}");

        CheckInternetConnection.ShowToast(GameData.Instance.translations.bluetoothToasts.failed);
    }
    private void BTReconnect()
    {
        Debug.Log($"{nameof(InitializeConnection)} - {nameof(BTReconnect)}");

        string deviceAddress = PlayerPrefs.GetString("BTDevAddress");
        BluetoothForAndroid.ConnectToServerByAddress(serverUUID, deviceAddress);
    }
    private void BTReceiveRotate3DModel(byte[] data)
    {
        RotationMsg rotationMsg = MemoryPackSerializer.Deserialize<RotationMsg>(data);

        CommConstants.x = rotationMsg.x;
        CommConstants.y = rotationMsg.y;
        CommConstants.z = rotationMsg.z;
        CommConstants.animal_id = rotationMsg.animal_id;
    }
}