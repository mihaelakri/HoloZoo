using SVSBluetooth;
using UnityEngine;
using UnityEngine.UI;

public class BTButtonController : MonoBehaviour
{
    [SerializeField]
    GameObject notConnectedOverlay, connectedOverlay;

    [SerializeField]
    Text deviceName;

    void Start()
    {
        BluetoothForAndroid.DeviceConnected += () =>
        {
            notConnectedOverlay.SetActive(false);
        };
    }

    public void CloseOverlays()
    {
        notConnectedOverlay.SetActive(false);
        connectedOverlay.SetActive(false);
    }

    public void ShowOverlay()
    {
        bool isConnected = InitializeConnection.Instance.IsConnected();

        if (isConnected)
        {
            connectedOverlay.SetActive(true);
            deviceName.text = PlayerPrefs.GetString("BTDevName", "-");
        }
        else
        {
            notConnectedOverlay.SetActive(true);
            InitializeConnection.Instance.EnableBluetoothDiscoverability();
            InitializeConnection.Instance.ServerStart();
        }
    }

    public void Disconnect()
    {
        InitializeConnection.Instance.Disconnect();
        CloseOverlays();
    }
}