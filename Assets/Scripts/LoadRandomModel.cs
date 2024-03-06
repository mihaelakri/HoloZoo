using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class LoadRandomModel : MonoBehaviour
{
    public GameObject variableForPrefab;
    public String model_url;
    public GameObject instantiatedObject;
    public static event System.Action OnModelLoaded;

    public void LoadAnimal(int id)
    {
        StartCoroutine(GetModel(id));
        //OnModelLoaded?.Invoke();
    }

    IEnumerator GetModel(int id){

        WWWForm form = new WWWForm();
        form.AddField("id_model", id);

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"animal_view.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Animal model error: "+www.error);
                }
            else
                {
                    model_url = (www.downloadHandler.text);
                    Debug.Log("Animal model found: " + www.downloadHandler.text);
                    //Debug.Log("Model URL: " + model_url);

                    variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));

                    if (variableForPrefab == null)
                    {
                        Debug.LogError("Prefab not found with name: " + model_url);
                        //yield break; // Exit the coroutine to avoid further issues
                    }

                GameObject obj = GameObject.Find("Model");
                if (obj != null)
                {
                    instantiatedObject = Instantiate(variableForPrefab, new Vector3(0, 0, 0), Quaternion.identity, obj.transform);
                }
                else
                {
                    Debug.LogWarning("GameObject with tag '3d-obj' not found in the scene.");
                }

                BoxCollider boxCollider = instantiatedObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1f, 1f, 1f);
                instantiatedObject.layer = LayerMask.NameToLayer("Animal");
                Rigidbody rb = (Rigidbody)instantiatedObject.gameObject.AddComponent(typeof(Rigidbody));
                instantiatedObject.GetComponent<Rigidbody>().useGravity = false;
                Debug.Log("Object to be resized.");
                ResizeUtility.ResizeObject(instantiatedObject, CommConstants.rotationMsg.initial_size, 2.0f);
                Debug.Log("Object resized.");

                // Add script for mouse rotate if control is set to mouse
                if (CommConstants.rotationMsg.control_type == 2){
                        instantiatedObject.AddComponent<MouseRotate>();
                    }

                    /*
                    foreach (var s in www.GetResponseHeader("Set-Cookie").Split(';')) {
                        if(s.Contains("holozoo_session")){
                            CommConstants.Auth = s.Substring(s.IndexOf("holozoo_session")).Split('=')[1].Split(';')[0];
                            Debug.Log(CommConstants.Auth);

                        } else if (s.Contains("XSRF-TOKEN")){
                            CommConstants.XSRF = s.Substring(s.IndexOf("XSRF-TOKEN")).Split('=')[1].Split(';')[0];
                            Debug.Log(CommConstants.XSRF);

                        }
                    }
                    */
                }
        }

       
    }

    public void DestroyInstantiatedObject(){
        if (instantiatedObject != null)
        {
            // Perform any cleanup or necessary actions before destroying the object
            // For example, removing components or releasing resources

            // Destroy the instantiated object
            Destroy(instantiatedObject);

            // Set the reference to null after destroying
            instantiatedObject = null;
        }
    }

    public GameObject getFirstChild(GameObject ParentObject) {
        return ParentObject.transform.GetChild(0).gameObject;
    }



}
