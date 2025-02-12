using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class WorldMapTestAuth : MonoBehaviour
{
    public GameObject variableForPrefab;
    public String model_url;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetModel());
    }

    IEnumerator GetModel(){

        string id_animal = "0";
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
                    Debug.Log(www.downloadHandler.text);

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
