using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PusherClient;
using SVSBluetooth;

public static class CommConstants
{
    // public const string ServerURL = "http://localhost:8000/";
    public const string ServerURL = "http://localhost/holozoo/";
    public static string Auth = "";
    public static string XSRF = "";

    public static string IdUser = "";

    public static Pusher pusher;
    public static Channel channel;
    public static  bool is_websocket_open = false;
    public static string conn_method;
    public static bool is_BTConnected = false;
    public static string paired_BT_server;

    public static float x=0f,y=0f, z=0f;
    public static string new_animal_id;
    public static int player_id;

    public static int control_type = 1; 
}