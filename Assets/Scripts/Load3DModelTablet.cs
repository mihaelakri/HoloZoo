using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Load3DModelTablet : MonoBehaviour
{
    public GameObject variableForPrefab;
    public String model_url;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetModel());
    }

    public static IEnumerator GetModel()
    {
        string model_url;

        Debug.Log("id_animal: " + CommConstants.animal_id);

        WWWForm form = new WWWForm();
        form.AddField("id_model", CommConstants.animal_id);

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "animal_view.php", form))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (CommConstants.animal_id == 0)
                {
                    model_url = "WorldMapGlobe";
                }
                else
                {
                    model_url = www.downloadHandler.text;
                }

                GameObject parent = GameObject.FindGameObjectWithTag("3d-obj");
                if (parent.transform.childCount > 0)
                {
                    // Destroy(parent.transform.GetChild(0).gameObject);
                    foreach (Transform child in parent.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

                Debug.Log(www.downloadHandler.text);

                GameObject variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
                GameObject instantiatedObject = Instantiate(variableForPrefab, new Vector3(0, -1, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d-obj").transform);
                //Instantiate(variableForPrefab, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d-obj").transform);

                if (model_url == "WorldMapGlobe")
                {
                    yield break;
                }

                BoxCollider boxCollider = instantiatedObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1f, 1f, 1f);
                instantiatedObject.layer = LayerMask.NameToLayer("Animal");
                Rigidbody rb = (Rigidbody)instantiatedObject.gameObject.AddComponent(typeof(Rigidbody));
                ResizeUtility.ResizeObjectTablet(instantiatedObject);
                // instantiatedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                instantiatedObject.GetComponent<Rigidbody>().useGravity = false;
                instantiatedObject.GetComponent<Rigidbody>().isKinematic = true;
                instantiatedObject.GetComponent<Rigidbody>().detectCollisions = false;
            }
        }
    }


}
