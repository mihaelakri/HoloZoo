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
        // WWWForm form = new WWWForm();
        // form.AddField("id_model", PlayerPrefs.GetString("id_animal"));
        string id_animal = PlayerPrefs.GetString("id_animal");

        using (UnityWebRequest www = UnityWebRequest.Get(CommConstants.ServerURL+"animal/model/"+id_animal)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Animal model error: "+www.error);
                }
            else
                {
                    model_url = (www.downloadHandler.text);
                    Debug.Log("Animal model found: "+www.downloadHandler.text);
                    variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
                    Instantiate(variableForPrefab, new Vector3(0, 2f, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d-obj").transform);

                    // foreach (var s in www.GetResponseHeaders()) {
                    //     Debug.Log("s=" + s);
                    // }
                    foreach (var s in www.GetResponseHeader("Set-Cookie").Split(';')) {
                        if(s.Contains("holozoo_session")){
                            CommConstants.Auth = s.Substring(s.IndexOf("holozoo_session")).Split('=')[1].Split(';')[0];
                            Debug.Log(CommConstants.Auth);

                        } else if (s.Contains("XSRF-TOKEN")){
                            CommConstants.XSRF = s.Substring(s.IndexOf("XSRF-TOKEN")).Split('=')[1].Split(';')[0];
                            Debug.Log(CommConstants.XSRF);

                        }
                    }
                    // Debug.Log(www.GetResponseHeader("Set-Cookie"));
                    // CommConstant.Auth = www.GetRequestHeader("Cookie");
                }
        }
    }

    
}
