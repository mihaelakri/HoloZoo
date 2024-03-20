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
using SVSBluetooth;
using System.Threading;
using CommunicationMsgs;

#if UNITY_STANDALONE_WIN
using Leap;
using Leap.Unity;
#endif

public class RotateModel : MonoBehaviour
{
    public Slider bottomSlider;
    public Slider sideSlider;
    public GameObject model;
    // public GameObject target;
    public float sliderLastX = 0, sliderLastY = 0;
    public RotateModel instance = null;
    public Quaternion initialRotation;
    public float rotationSpeed = 1f;
    private string old_animal_id;
    Scene m_Scene;
    string sceneName;

    // Usage time tracking
    float currentLeapUsage = 0f;
    bool leapUsed = false;


#if UNITY_STANDALONE_WIN
    private LeapServiceProvider leapProvider;
#endif



    void Start()
    {

        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        CommConstants.state.player_id = PlayerPrefs.GetInt("ID");
        CommConstants.rotation.OnRotationUpdated += RotateModel_OnRotationUpdated;
        CommConstants.requestLeapTimeMsg.OnRequestLeapTimeUpdated += SendLeapTime;

        if (sceneName == "HologramGlobe")
        {
            rotateGlobeModel(new Vector3(339.671448f, 121.115952f, 348.156769f));
        }
        else if (sceneName == "HologramTablet")
        {

#if UNITY_STANDALONE_WIN
         leapProvider = FindObjectOfType<LeapServiceProvider>();

         if (leapProvider == null)
         {
             Debug.LogError("LeapServiceProvider not found in the scene.");
         }
#endif
            RotateModelLeap(model);
        }




        if (sceneName == "HologramQuizIntro" || sceneName == "HologramQuiz")
        {

            CommConstants.rotation.x = 0f;
            CommConstants.rotation.y = 0f;
            CommConstants.rotation.z = 0f;
            bottomSlider.value = 0;
            sideSlider.value = 0;

        }

        CommConstants.connection.SendData(CommConstants.rotation);
    }

    public void RotateModel_OnRotationUpdated()
    {
        System.Diagnostics.Debug.WriteLine("RotateModel_OnRotationUpdated called");
        Vector3 localRotation = new Vector3(CommConstants.rotation.x, CommConstants.rotation.y, CommConstants.rotation.z);
        model.transform.GetChild(0).gameObject.transform.eulerAngles = transform.eulerAngles + localRotation;
    }

    IEnumerator SwapModel()
    {
        GameObject parent = GameObject.FindGameObjectWithTag("3d");
        Destroy(parent.transform.GetChild(0).gameObject);

        string id_animal = CommConstants.animalid.animal_id;
        // id_animal = "2";
        string model_url;

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "animal_view.php", id_animal))
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
            // if (this.old_animal_id != CommConstants.state.animal_id)
            // {
            //     StartCoroutine(SwapModel());
            //     this.old_animal_id = CommConstants.state.animal_id;
            // }

            try
            {

                if (CommConstants.animalid.animal_id == "0")
                {
                    Vector3 localRotation = new Vector3(CommConstants.rotation.x, CommConstants.rotation.y, CommConstants.rotation.z);
                    model.transform.GetChild(0).transform.eulerAngles = transform.eulerAngles + localRotation;
                }
                else if (CommConstants.state.start_quiz_flag == 1)
                {
                    initialRotation = model.transform.GetChild(0).gameObject.transform.rotation;
                    RotateModelLeap(model.transform.GetChild(0).gameObject);

                }
                else if (sceneName == "HologramTablet")
                {
                    Debug.Log("ff");
                    // System.Diagnostics.Debug.WriteLine("Rotatemodel elif 3");
                    initialRotation = model.transform.GetChild(0).gameObject.transform.rotation;

                    float leapUsageStart = Time.time;
                    RotateModelLeap(model.transform.GetChild(0).gameObject);

                    if (leapUsed)
                    {
                        currentLeapUsage += Time.time - leapUsageStart;
                        leapUsed = false;
                    }
                }


            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }


    }

    private float GetAndResetLeapUsage()
    {
        float usageCopy = currentLeapUsage;
        currentLeapUsage = 0f;
        return usageCopy;
    }

    public void SendLeapTime()
    {
        CommConstants.leapTimeMsg.leap_time = GetAndResetLeapUsage();
        CommConstants.connection.SendData(CommConstants.leapTimeMsg);
        System.Diagnostics.Debug.WriteLine("LeapTime sent");
    }

    public void rotateModel()
    {
        if (sceneName != "HologramGlobe")
        {
            CommConstants.rotation.x = bottomSlider.value;
            CommConstants.rotation.y = sideSlider.value;
            CommConstants.rotation.z = 0f;
        }



        CommConstants.connection.SendData(CommConstants.rotation);

    }

    public void rotateGlobeModel(Vector3 spherePosition)
    {
        CommConstants.rotation.x = spherePosition.x;
        CommConstants.rotation.y = spherePosition.y;
        CommConstants.rotation.z = spherePosition.z;
        // Debug.Log("Sphere x: "+spherePosition.x+" y: "+spherePosition.y+" z: "+spherePosition.z);
        this.rotateModel();
    }



    void RotateModelLeap(GameObject objectToRotate)
    {


#if UNITY_STANDALONE_WIN
        Frame frame = leapProvider.CurrentFrame;
        rotationSpeed = CommConstants.state.initial_rotation_speed;

        Debug.Log("radi leap");
        if (frame != null && frame.Hands.Count > 0)
        {
            Hand hand = frame.Hands[0];

            CommConstants.state.control_type = 1; // 1 leap
            rotationSpeed = CommConstants.state.initial_rotation_speed;


            string dominantDirection = GetDominantHandDirection(hand);
            Debug.Log(dominantDirection);
            if (dominantDirection.Equals("up"))
            {
                objectToRotate.transform.RotateAround(objectToRotate.transform.position, Vector3.up, 25 * Time.deltaTime * rotationSpeed);
            }
            else if (dominantDirection.Equals("down"))
            {
                objectToRotate.transform.RotateAround(objectToRotate.transform.position, Vector3.down, 25 * Time.deltaTime * rotationSpeed);
            }
            else if (dominantDirection.Equals("right"))
            {
                objectToRotate.transform.RotateAround(objectToRotate.transform.position, Vector3.right, 25 * Time.deltaTime * rotationSpeed);
            }
            else
            {
                objectToRotate.transform.RotateAround(objectToRotate.transform.position, Vector3.left, 25 * Time.deltaTime * rotationSpeed);
            }

            Vector3 eulerRotation = objectToRotate.transform.rotation.eulerAngles;

            CommConstants.rotation.x = eulerRotation.x;
            CommConstants.rotation.y = eulerRotation.y;
            CommConstants.rotation.z = eulerRotation.y;

            CommConstants.connection.SendData(CommConstants.rotation);
            
            leapUsed = true;
        }
        else
        {
#endif

            if (CommConstants.state.control_type != 3)
            {
                CommConstants.state.control_type = 2;
            }
            // Debug.Log("Control type: " + CommConstants.state.control_type + " Leap Motion off");

            Quaternion localRotation = Quaternion.Euler(CommConstants.rotation.x, CommConstants.rotation.y, CommConstants.rotation.z);
            //Debug.Log("uslo x: " + CommConstants.rotation.x);
            model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;

#if UNITY_STANDALONE_WIN
        }
#endif
    }


#if UNITY_STANDALONE_WIN
         public string GetDominantHandDirection(Hand hand)
    {
        Vector3 handVelocity = hand.PalmVelocity;

        float absVelocityX = Mathf.Abs(handVelocity.x);
        float absVelocityY = Mathf.Abs(handVelocity.y);

        if (absVelocityX > absVelocityY && (hand.IsLeft || hand.IsRight)) //horizontal
        {
            return handVelocity.x > 0 ? "right" : "left";
        }
        else //vertical
        {
            return handVelocity.y > 0 ? "up" : "down";
        }

    }
#endif

}