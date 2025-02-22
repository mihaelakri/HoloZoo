using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Load3Dmodel : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetModel());
    }

    public static IEnumerator GetModel()
    {

        Debug.Log("Load3DModel id_animal: " + CommConstants.animal_id);
        WWWForm form = new WWWForm();
        form.AddField("id_model", CommConstants.animal_id);
        //string id_animal = PlayerPrefs.GetString("id_animal");

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "animal_view.php", form))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Animal model error: " + www.error);
            }
            else
            {
                if (SceneManager.GetActiveScene().name == "HologramTablet")
                {
                    GameObject parent = GameObject.FindGameObjectWithTag("3d-obj");
                    Destroy(parent.transform.GetChild(0).gameObject);
                }

                string model_url = (www.downloadHandler.text);
                Debug.Log("Animal model found: " + www.downloadHandler.text);
                GameObject variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
                GameObject instantiatedObject = Instantiate(variableForPrefab, new Vector3(0, 2f, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d-obj").transform);

                BoxCollider boxCollider = instantiatedObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1f, 1f, 1f);
                instantiatedObject.layer = LayerMask.NameToLayer("Animal");
                Rigidbody rb = (Rigidbody)instantiatedObject.gameObject.AddComponent(typeof(Rigidbody));
                instantiatedObject.GetComponent<Rigidbody>().useGravity = false;
                instantiatedObject.GetComponent<Rigidbody>().isKinematic = true;
                instantiatedObject.GetComponent<Rigidbody>().detectCollisions = false;

                ResizeUtility.ResizeObject(instantiatedObject);
            }
        }
    }


}
