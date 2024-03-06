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

    public static RotationMsg rotationMsg = new RotationMsg(0f, 0f, 0f, 0, "");

    public static int control_type = 0; 
    public static float initial_size = 1f;
    public static float finish_size = 0f;
    public static float initial_rotation_speed = 1f; 
    public static int start_quiz_flag = 0;
}