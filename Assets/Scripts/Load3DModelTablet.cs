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
    string id_animal;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetModel());

    }

    IEnumerator GetModel(){
        Debug.Log("id_animal"+PlayerPrefs.GetString("id_animal"));
        if(PlayerPrefs.GetString("id_animal")!= "")
        {
            id_animal = PlayerPrefs.GetString("id_animal", "1");
        }
        else
        {
            id_animal = CommConstants.animalid.animal_id;
        }
        

        WWWForm form = new WWWForm();
        form.AddField("id_model", id_animal);

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "animal_view.php", form)){
        //using (UnityWebRequest www = UnityWebRequest.Get(CommConstants.ServerURL+"animal/model/"+id_animal)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    if (id_animal == "0") {
                        model_url = "WorldMapGlobe";
                    } else {
                        model_url = (www.downloadHandler.text);
                    }
                    Debug.Log(www.downloadHandler.text);
                    variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
                    GameObject instantiatedPrefab = Instantiate(variableForPrefab, new Vector3(0, -1, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d").transform);
                    instantiatedPrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    //Instantiate(variableForPrefab, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d").transform);

                    foreach (var s in www.GetResponseHeader("Set-Cookie").Split(';')) {
                        if(s.Contains("holozoo_session")){
                            CommConstants.Auth = s.Substring(s.IndexOf("holozoo_session")).Split('=')[1].Split(';')[0];
                            Debug.Log(CommConstants.Auth);

                        } else if (s.Contains("XSRF-TOKEN")){
                            CommConstants.XSRF = s.Substring(s.IndexOf("XSRF-TOKEN")).Split('=')[1].Split(';')[0];
                            Debug.Log(CommConstants.XSRF);

                        }
                    }
                }
        }
    }

    
}
