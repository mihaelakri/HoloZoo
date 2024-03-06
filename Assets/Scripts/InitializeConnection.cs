using System;
using System.Globalization;
using UnityEngine;

/// <summary>
/// This script should be attached to a GameObject in a starting scene after a communication method is chosen. 
/// Currently it is used in the <c>Home</c> scene on the object <c>CommHelper</c>.
/// </summary>
public class InitializeConnection : MonoBehaviour
{
    ConnectionBase connection;

    // Start is called before the first frame update
    async void Start()
    {
        CommConstants.conn_method = PlayerPrefs.GetString("conn_method");
        Debug.Log("conn_method: "+CommConstants.conn_method);
        
        if (CommConstants.conn_method == "websocket") {
            connection = new ConnectionPusher();
        } else {
            connection = new ConnectionBluetooth();
        }

        CommConstants.connection = connection;

        await connection.Initialize();
    }

}
