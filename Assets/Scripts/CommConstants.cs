using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PusherClient;
using SVSBluetooth;

public static class CommConstants
{
    public static ConnectionBase connection;
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

    public static int control_type = 0; 
    public static float initial_size = 1f;
    public static float finish_size = 0f;
    public static float initial_rotation_speed = 1f; 
    public static int start_quiz_flag = 0;
}