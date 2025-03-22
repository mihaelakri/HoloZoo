using SVSBluetooth;
using UnityEngine;
using UnityEngine.UI;

public class BTButtonControllerTablet : MonoBehaviour
{
    [SerializeField]
    GameObject unpairedOverlay, pairedOverlay, connectedOverlay;

    [SerializeField]
    Text pairedDeviceText, connectedDeviceText;

    void OnEnable()
    {
        BluetoothForAndroid.DeviceConnected += CloseUnpairedOverlay;
    }

    void OnDisable()
    {
        BluetoothForAndroid.DeviceConnected -= CloseUnpairedOverlay;
    }

    private void CloseUnpairedOverlay()
    {
        unpairedOverlay.SetActive(false);
    }

    public void CloseOverlays()
    {
        CloseUnpairedOverlay();
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