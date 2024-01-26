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
using Leap;
using Leap.Unity;
public class RotateModel : MonoBehaviour
{

    public Slider bottomSlider;
    public Slider sideSlider;
    public GameObject model;
    public GameObject target;
    public float sliderLastX = 0, sliderLastY = 0;
    private LeapServiceProvider leapProvider;
    private float updateFrequency = 1f / 9f; // 10Hz is max. allowed by Pusher
    private float lastUpdateTime = 0f;
    public RotateModel instance = null;
    public Quaternion initialRotation;
    public float rotationSpeed = 0.5f;
    private string old_animal_id;
    Scene m_Scene;
    string sceneName;
   // public float sliderSpeed = 1.0f;
   // public float velocityThreshold = 0.1f;
   // public float positionScale = 0.1f; // Adjust based on the expected range of hand positions

    private Vector3 previousHandPosition;
    private class RotationMsg
    {
        public string x;
        public string y;
        public string z;
        public int player_id;
        public string animal_id;

        public RotationMsg(string x, string y, string z, int player_id, string animal_id)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.player_id = player_id;
            this.animal_id = animal_id;
        }
    }

    private class PusherRotationMsg
    {
        public string @event;
        public string data;
        public string channel;
    }

    void Start()
    {

        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        CommConstants.player_id = PlayerPrefs.GetInt("ID");

        if (sceneName == "HologramGlobe")
        {
            rotateGlobeModel(new Vector3(339.671448f, 121.115952f, 348.156769f));
        }
        else if (sceneName == "HologramAnimalMobile")
        {
            rotateModel();
        }


        leapProvider = FindObjectOfType<LeapServiceProvider>();

        if (leapProvider == null)
        {
            Debug.LogError("LeapServiceProvider not found in the scene.");
        }

    }

  
    IEnumerator SwapModel()
    {
        GameObject parent = GameObject.FindGameObjectWithTag("3d");
        Destroy(parent.transform.GetChild(0).gameObject);

        string id_animal = CommConstants.new_animal_id;
        // id_animal = "2";
        string model_url;
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/HoloZoo/animal_view.php", id_animal))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (id_animal == "0")
                {
                    model_url = "WorldMapGlobe";
                }
                else
                {
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
        if (sceneName != "HologramGlobe")
        {
            if (this.old_animal_id != CommConstants.new_animal_id)
            {
                StartCoroutine(SwapModel());
                this.old_animal_id = CommConstants.new_animal_id;
            }

            try
            {
             
                if (CommConstants.new_animal_id == "0")
                {
                    Vector3 localRotation = new Vector3(CommConstants.x, CommConstants.y, CommConstants.z);
                    model.transform.GetChild(0).transform.eulerAngles = transform.eulerAngles + localRotation;
                }
                else if (CommConstants.control_type == 1)
                {
                    initialRotation = model.transform.GetChild(0).gameObject.transform.rotation;
                    RotateModelLeap(model.transform.GetChild(0).gameObject);

                }
             

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }


    }


    public void rotateModel()
    {
        if (sceneName != "HologramGlobe")
        {
            CommConstants.x = bottomSlider.value;
            CommConstants.y = sideSlider.value;
            CommConstants.z = 0f;
        }

        if (CommConstants.conn_method == "websocket")
        {
            if (!CommConstants.is_websocket_open)
            {
                Debug.Log("Websocket not open");
                return;
            }

            float timeSinceLastUpdate = Time.time - lastUpdateTime;

            if (timeSinceLastUpdate >= updateFrequency)
            {
                StartCoroutine(SendRotate3DModel());
                lastUpdateTime = Time.time;
            }
        }
        else
        {    // Bluetooth
            BTSendRotate3DModel();
        }

    }

    public void rotateGlobeModel(Vector3 spherePosition)
    {
        CommConstants.x = spherePosition.x;
        CommConstants.y = spherePosition.y;
        CommConstants.z = spherePosition.z;
        // Debug.Log("Sphere x: "+spherePosition.x+" y: "+spherePosition.y+" z: "+spherePosition.z);
        this.rotateModel();
    }

    IEnumerator SendRotate3DModel()
    {
        // Debug.Log(PlayerPrefs.GetString("id_animal"));

        RotationMsg rotationMsg = new RotationMsg(
            CommConstants.x.ToString(),
            CommConstants.y.ToString(),
            CommConstants.z.ToString(),
            CommConstants.player_id,
            PlayerPrefs.GetString("id_animal", "1")
        );

        CommConstants.channel.Trigger(
            "client-rotation" + CommConstants.player_id,
            JsonUtility.ToJson(rotationMsg)
        );
        yield return 0;
    }

    private void BTSendRotate3DModel()
    {
       // Debug.Log("Bluetooth - BTSendRotate3DModel");
        RotationMsg rotationMsg = new RotationMsg(
            CommConstants.x.ToString(),
            CommConstants.y.ToString(),
            CommConstants.z.ToString(),
            CommConstants.player_id,
            PlayerPrefs.GetString("id_animal", "1")
        );
        BluetoothForAndroid.WriteMessage(JsonUtility.ToJson(rotationMsg));
    }

    void RotateModelLeap(GameObject objectToRotate)
    {


        Frame frame = leapProvider.CurrentFrame;

        if (frame != null && frame.Hands.Count > 0)
        {
            Hand hand = frame.Hands[0]; // Assuming you're interested in the first detected hand

            // Get the hand's velocity along the Y-axis
            float yVelocity = hand.PalmVelocity.y;

            // Check if the hand is moving up or down based on the velocity
            float rotationAngleY = yVelocity * rotationSpeed * Time.deltaTime;
            if (yVelocity > 0)
            {
                objectToRotate.transform.RotateAround(objectToRotate.transform.position, Vector3.up, 25 * Time.deltaTime);
            }
            else
            {
                objectToRotate.transform.RotateAround(objectToRotate.transform.position, Vector3.up, -25 * Time.deltaTime);
            }

            // Similarly for X-axis rotation
            float xVelocity = hand.PalmVelocity.x;

            float rotationAngleX = xVelocity * rotationSpeed * Time.deltaTime;

            if (xVelocity > 0)
            {
                objectToRotate.transform.RotateAround(objectToRotate.transform.position, Vector3.right, 25 * Time.deltaTime);
            }
            else
            {
                objectToRotate.transform.RotateAround(objectToRotate.transform.position, Vector3.right, -25 * Time.deltaTime);
            }



            // Update slider value based on mapped y and y position
     
            Vector3 eulerRotation = objectToRotate.transform.rotation.eulerAngles;

            bottomSlider.value = eulerRotation.x;
            sideSlider.value = eulerRotation.y;


        }
        else {
            Quaternion localRotation = Quaternion.Euler(CommConstants.x, CommConstants.y, CommConstants.z);
            model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;
        }

    }



}