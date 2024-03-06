using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PusherClient;
using SVSBluetooth;
using CommunicationMsgs;

public static class CommConstants
{
    public static ConnectionBase connection;
    // public const string ServerURL = "http://localhost:8000/";
    public const string ServerURL = "http://localhost/holozoo/";
    public static string Auth = "";
    public static string XSRF = "";

    public static string IdUser = "";

    public static StateMsg state = new StateMsg(0f, 0f, 0f, 0, "", 0, 1f, 0f, 1f, 0);
}