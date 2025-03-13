using SVSBluetooth;
using UnityEngine;
using UnityEngine.UI;

public class BTButtonControllerTablet : MonoBehaviour
{
    [SerializeField]
    GameObject unpairedOverlay, pairedOverlay, connectedOverlay;

    [SerializeField]
    Text pairedDeviceText, connectedDeviceText;

    void Start()
    {
        BluetoothForAndroid.DeviceConnected += () =>
        {
            unpairedOverlay.SetActive(false);
        };
    }

    public void CloseOverlays()
    {
        unpairedOverlay.SetActive(false);
        pairedOverlay.SetActive(false);
        connectedOverlay.SetActive(false);
    }

    public void ShowOverlay()
    {
        bool isPaired = InitializeConnection.Instance.IsPaired();
        bool isConnected = InitializeConnection.Instance.IsConnected();

        if (isConnected)
        {
            connectedOverlay.SetActive(true);
            connectedDeviceText.text = PlayerPrefs.GetString("BTDevName");
        }
        else if (isPaired)
        {
            pairedOverlay.SetActive(true);
            pairedDeviceText.text = PlayerPrefs.GetString("BTDevName");
        }
        else
        {
            unpairedOverlay.SetActive(true);
        }
    }

    public void Connect()
    {
        InitializeConnection.Instance.ClientConnect(true);
        CloseOverlays();
    }

    public void Disconnect()
    {
        InitializeConnection.Instance.Disconnect();
        CloseOverlays();
    }

    public void ForgetDevice()
    {
        InitializeConnection.Instance.ForgetDevice();
        CloseOverlays();
    }
}