using CommMsgs;
using MemoryPack;
using SVSBluetooth;
using System;
using UnityEngine;
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

    void Update()
    {

        if (sceneName != "HologramGlobe")
        {
            if (sceneName == "HologramTablet" && old_animal_id != CommConstants.animal_id)
            {
                // StartCoroutine(SwapModel());
                StartCoroutine(Load3DModelTablet.GetModel());
                old_animal_id = CommConstants.animal_id;
            }

            if (CommConstants.animal_id == 0)
            {
                model.transform.GetChild(0).transform.rotation = Quaternion.Euler(CommConstants.x, CommConstants.y, CommConstants.z);
                return;
            }

            try
            {
                float interpolationFactor = 10f;

                if (CommConstants.animal_id == 0)   // Globe
                {
                    interpolationFactor = 10f;
                }

                Quaternion currentRotation = model.transform.GetChild(0).transform.rotation;
                Quaternion targetRotation = Quaternion.Euler(CommConstants.x, CommConstants.y, CommConstants.z);
                Quaternion smoothedRotation;

                if (Quaternion.Dot(currentRotation, targetRotation) < 0f)
                {
                    smoothedRotation = Quaternion.RotateTowards(currentRotation, targetRotation, -30f);
                }
                else
                {
                    smoothedRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * interpolationFactor);
                }

                model.transform.GetChild(0).transform.rotation = smoothedRotation;

                // if (CommConstants.x != lastX)
                // {
                //     Debug.Log("update rotation current: " + currentRotation.eulerAngles
                //     + "\t   target: " + targetRotation.eulerAngles
                //     + "\t\tdot: " + Quaternion.Dot(currentRotation, targetRotation)
                //     + "\t\t\tangle: " + Quaternion.Angle(currentRotation, targetRotation)
                //     // + "\t\tslerp angle: " + Quaternion.Angle(currentRotation, smoothedRotation) 
                //     );
                // }
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
        // Debug.Log("Bluetooth - BTSendRotate3DModel: " + serializedMsg);
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

        if (Time.time - lastUpdateTime >= 0.04)  // 25Hz send rate limit
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
