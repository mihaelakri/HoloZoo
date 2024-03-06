using System;
using System.Globalization;
using UnityEngine;

/// <summary>
/// This script should be attached to a GameObject in a starting scene after a communication method is chosen. 
/// Currently it is used in the <c>Home</c> scene on the object <c>CommHelper</c>.
/// </summary>
public class InitializeConnection : MonoBehaviour
{
    public ConnectionBase connection;
    public string conn_method;

    // Start is called before the first frame update
    async void Start()
    {
        conn_method = PlayerPrefs.GetString("conn_method");
        Debug.Log("conn_method: "+conn_method);
        
        if (conn_method == "websocket") {
            connection = new ConnectionPusher();
        } else {
            connection = new ConnectionBluetooth();
        }

        CommConstants.connection = connection;

        await connection.Initialize();
    }

}
