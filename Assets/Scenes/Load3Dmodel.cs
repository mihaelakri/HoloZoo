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
    public GameObject variableForPrefab;
    public String model_url;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetModel());
    }

    IEnumerator GetModel(){

        Debug.Log(PlayerPrefs.GetString("id_animal"));
        WWWForm form = new WWWForm();
        form.AddField("id_model", PlayerPrefs.GetString("id_animal"));
        string id_animal = PlayerPrefs.GetString("id_animal");

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
                    GameObject instantiatedObject = Instantiate(variableForPrefab, new Vector3(0, 2f, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d-obj").transform);
                    
                    // Calculate half of the model's height
                    RectTransform rt = (RectTransform)instantiatedObject.transform;
                    float width = rt.rect.width;
                    float height = rt.rect.height;
                    Debug.Log("height: " +height);
                    
                    // Adding a BoxCollider to the instantiated object for mouse drag
                    BoxCollider boxCollider = instantiatedObject.AddComponent<BoxCollider>();
                    // Set the collider size (if not already set)
                    boxCollider.size = new Vector3(2f, 2f, 2f); // Adjust this to your desired default size
                    // Adjust collider position to match the GameObject position
                    instantiatedObject.AddComponent<MouseRotate>();

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

    
}
