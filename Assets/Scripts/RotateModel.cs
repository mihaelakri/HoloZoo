using CommMsgs;
using MemoryPack;
using SVSBluetooth;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RotateModel : MonoBehaviour
{
    public Slider bottomSlider;
    public Slider sideSlider;
    public GameObject model;
    public GameObject target;
    private int old_animal_id;
    Scene m_Scene;
    string sceneName;
    private float lastUpdateTime;

    void Start()
    {
        lastUpdateTime = Time.time;
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;

        if (sceneName == "HologramGlobe")
        {
            rotateGlobeModel(new Vector3(339.671448f, 121.115952f, 348.156769f));
        }
        else if (sceneName == "HologramAnimalMobile")
        {
            rotateModel();
        }
    }

    IEnumerator SwapModel()
    {
        int id_animal = CommConstants.animal_id;

        WWWForm form = new WWWForm();
        form.AddField("id_model", PlayerPrefs.GetString("id_animal"));
        

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"animal_view.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string model_url;
                if (id_animal == 0)
                {
                    model_url = "WorldMapGlobe";
                }
                else
                {
                    model_url = www.downloadHandler.text;
                }
                Debug.Log("SwapModel - resp: " + www.downloadHandler.text);
                GameObject parent = GameObject.FindGameObjectWithTag("3d");
                Destroy(parent.transform.GetChild(0).gameObject);

                GameObject variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
                Instantiate(variableForPrefab, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d").transform);
            }
        }

    }
    void Update()
    {
        if (sceneName != "HologramGlobe")
        {
            if (old_animal_id != CommConstants.animal_id)
            {
                StartCoroutine(SwapModel());
                old_animal_id = CommConstants.animal_id;
            }

            try
            {
                if (CommConstants.animal_id == 0)
                {
                    Vector3 localRotation = new Vector3(CommConstants.x, CommConstants.y, CommConstants.z);
                    model.transform.GetChild(0).transform.eulerAngles = transform.eulerAngles + localRotation;
                }
                else
                {
                    Quaternion localRotation = Quaternion.Euler(CommConstants.x, CommConstants.y, CommConstants.z);
                    model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public void BTSendRotate3DModel()
    {
        byte[] serializedMsg = MemoryPackSerializer.Serialize(CommConstants.rotationMsg);
        Debug.Log("Bluetooth - BTSendRotate3DModel: " + serializedMsg);
        lastUpdateTime = Time.time;
        BluetoothForAndroid.WriteMessage(serializedMsg);
    }

    public void rotateModel()
    {
        if (sceneName != "HologramGlobe")
        {
            CommConstants.x = sideSlider.value;
            CommConstants.y = bottomSlider.value;
            CommConstants.z = 0f;

            CommConstants.rotationMsg = new RotationMsg(CommConstants.x, CommConstants.y, CommConstants.z, CommConstants.animal_id);
        }

        if (Time.time - lastUpdateTime >= 0.025)  // 40Hz send rate limit
        {
            BTSendRotate3DModel();
        }
    }

    public void rotateGlobeModel(Vector3 spherePosition)
    {
        CommConstants.x = spherePosition.x;
        CommConstants.y = spherePosition.y;
        CommConstants.z = spherePosition.z;

        CommConstants.rotationMsg = new RotationMsg(CommConstants.x, CommConstants.y, CommConstants.z, CommConstants.animal_id);
        rotateModel();
    }
}
