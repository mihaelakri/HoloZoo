using SVSBluetooth;
using UnityEngine;
using UnityEngine.UI;

public class BTButtonController : MonoBehaviour
{
    [SerializeField]
    GameObject notConnectedOverlay, connectedOverlay;

    [SerializeField]
    Text deviceName;

    void OnEnable()
    {
        BluetoothForAndroid.DeviceConnected += CloseNotConnectedOverlay;
    }

    void OnDisable()
    {
        BluetoothForAndroid.DeviceConnected -= CloseNotConnectedOverlay;
    }

    private void CloseNotConnectedOverlay()
    {
        notConnectedOverlay.SetActive(false);
    }

    public void CloseOverlays()
    {
        CloseNotConnectedOverlay();
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