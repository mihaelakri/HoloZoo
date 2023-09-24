using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using PusherClient;
using SVSBluetooth;
using System.Threading;

public class RotateModel : MonoBehaviour
{

    public Slider bottomSlider;
    public Slider sideSlider;
    public GameObject model;
    public GameObject target;
    // public float x=0f,y=0f, z=0f;
    public float sliderLastX=0, sliderLastY=0;

    private float updateFrequency = 1f / 9f; // 10Hz is max. allowed by Pusher
    private float lastUpdateTime = 0f;
    public RotateModel instance = null;
    // private Pusher pusher;
    // private Channel channel;
    // private bool is_websocket_open = false;
    // private int player_id;
    // private string conn_method;
    // bool is_BTConnected = false;
    // private string new_animal_id;
    private string old_animal_id;
    // private string paired_BT_server;
    Scene m_Scene;
    string sceneName;

    private class RotationMsg {
        public string x;
        public string y;
        public string z;
        public int player_id;
        public string animal_id;

        public RotationMsg(string x, string y, string z, int player_id, string animal_id){
            this.x = x;
            this.y = y;
            this.z = z;
            this.player_id = player_id;
            this.animal_id = animal_id;
        }
    }

    private class PusherRotationMsg {
        public string @event;
        public string data;
        public string channel;
    }

    async Task Start()
    {
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        CommConstants.player_id = PlayerPrefs.GetInt("ID");


        if (sceneName == "HologramGlobe") {
            rotateGlobeModel(new Vector3(339.671448f,121.115952f,348.156769f));
        } else if (sceneName == "HologramAnimalMobile") {
            rotateModel();
        }
    }
    IEnumerator SwapModel() {
        GameObject parent = GameObject.FindGameObjectWithTag("3d");
        Destroy(parent.transform.GetChild(0).gameObject);

        string id_animal = CommConstants.new_animal_id;
        // id_animal = "2";
        string model_url;
        using (UnityWebRequest www = UnityWebRequest.Get(CommConstants.ServerURL+"animal/model/"+id_animal)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    if (id_animal == "0") {
                        model_url = "WorldMapGlobe";
                    } else {
                        model_url = (www.downloadHandler.text);
                    }
                    Debug.Log(www.downloadHandler.text);
                    GameObject variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
                    Instantiate(variableForPrefab, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d").transform);
                }
        }

    }
    void Update() 
    {
        if (sceneName != "HologramGlobe") {
            if (this.old_animal_id != CommConstants.new_animal_id) {
                StartCoroutine(SwapModel());
                this.old_animal_id = CommConstants.new_animal_id;
            }

            try {
                Quaternion localRotation = Quaternion.Euler(CommConstants.x, CommConstants.y, CommConstants.z);
                model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;
            } catch (Exception e) {
                Debug.Log(e);
            }
        }
    }


    public void rotateModel(){
        if (sceneName != "HologramGlobe") {
            CommConstants.x = sideSlider.value;
            CommConstants.y = bottomSlider.value;
            CommConstants.z = 0f;
        }
        // Quaternion localRotation = Quaternion.Euler(sideSlider.value,bottomSlider.value, 0f);
        // model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;

        // Debug.Log("rotate model");

        if (CommConstants.conn_method == "websocket") {
            if (!CommConstants.is_websocket_open){
                Debug.Log("Websocket not open");
                return;
            }

            float timeSinceLastUpdate = Time.time - lastUpdateTime;

            if (timeSinceLastUpdate >= updateFrequency)
            {
                StartCoroutine(SendRotate3DModel());
                lastUpdateTime = Time.time;
            }
        } else {    // Bluetooth
            BTSendRotate3DModel();
        }

    }

    public void rotateGlobeModel(Vector3 spherePosition) {
        CommConstants.x = spherePosition.x;
        CommConstants.y = spherePosition.y;
        CommConstants.z = spherePosition.z;
        // Debug.Log("Sphere x: "+spherePosition.x+" y: "+spherePosition.y+" z: "+spherePosition.z);
        this.rotateModel();
    }

    IEnumerator SendRotate3DModel(){
        // Debug.Log(PlayerPrefs.GetString("id_animal"));

        RotationMsg rotationMsg = new RotationMsg(
            CommConstants.x.ToString(),
            CommConstants.y.ToString(),
            CommConstants.z.ToString(),
            CommConstants.player_id,
            PlayerPrefs.GetString("id_animal", "1")
        );

        CommConstants.channel.Trigger(
            "client-rotation"+CommConstants.player_id,
            JsonUtility.ToJson(rotationMsg)
        );
        yield return 0;
    }

    private void BTSendRotate3DModel () {
        Debug.Log("Bluetooth - BTSendRotate3DModel");
        RotationMsg rotationMsg = new RotationMsg(
            CommConstants.x.ToString(),
            CommConstants.y.ToString(),
            CommConstants.z.ToString(),
            CommConstants.player_id,
            PlayerPrefs.GetString("id_animal", "1")
        );
        BluetoothForAndroid.WriteMessage(JsonUtility.ToJson(rotationMsg));
    }


}


