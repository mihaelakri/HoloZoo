using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using PusherClient;
using SVSBluetooth;
using CommunicationMsgs;


public static class CommConstants
{
    // public const string ServerURL = "http://localhost:8000/";
    public const string ServerURL = "https://holo.teserakt.biz/HoloZoo/";
    public static string Auth = "";
    public static string XSRF = "";

    public static string IdUser = "";

    //public static Pusher pusher;
    //public static Channel channel;
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


    public static ConnectionBase connection;
    public static StateMsg state = new StateMsg(0, 0, 1f, 0f, 1f, 0, 0,  false);
    public static RotationMsg rotation = new RotationMsg(0f, 0f, 0f, false);
    public static AnimalIdMsg animalid = new AnimalIdMsg("7", false);


}