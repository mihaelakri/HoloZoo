using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitializeConnection : MonoBehaviour
{
    public ConnectionBase connection;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.initUnityThread();
        Initalize();
        DontDestroyOnLoad(gameObject);
    }

    void Initalize()
    {
#if UNITY_ANDROID
        connection = gameObject.AddComponent<ConnectionBluetooth>();
#elif UNITY_WSA
            connection = gameObject.AddComponent<ConnectionBluetoothWindowsUWP>();
#elif UNITY_STANDALONE_WIN
            connection = gameObject.AddComponent<ConnectionBluetoothWindows32>();
#endif


        connection.Initialize();
        CommConstants.connection = connection;
    }


}