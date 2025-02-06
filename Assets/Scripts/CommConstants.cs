using CommMsgs;

public static class CommConstants
{
    public const string ServerURL = "http://localhost:8000/";
    // public const string ServerURL = "http://izelentrovic.com/";
    public static string Auth = "";
    public static string XSRF = "";

    public static string conn_method;
    public static bool is_BTConnected = false;
    public static string paired_BT_server;

    public static float x=0f,y=0f, z=0f;
    public static int animal_id = 0;
    public static RotationMsg rotationMsg = new RotationMsg(0f, 0f, 0f, 0);
}