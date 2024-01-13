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

    // Start is called before the first frame update
    public void LoadAnimal()
    {
        StartCoroutine(GetModel());
    }

    IEnumerator GetModel(){

        WWWForm form = new WWWForm();
        form.AddField("random_model", 1);

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
                    variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
                    instantiatedObject = Instantiate(variableForPrefab, new Vector3(0, 2f, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d-obj").transform);
                    
                    BoxCollider boxCollider = instantiatedObject.AddComponent<BoxCollider>();
                    boxCollider.size = new Vector3(2f, 2f, 2f);
                    
                    // Add script for mouse rotate if control is set to mouse
                    if(CommConstants.control_type == 2){
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

}
