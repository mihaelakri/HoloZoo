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

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/HoloZoo/animal_view.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    model_url = (www.downloadHandler.text);
                    Debug.Log(www.downloadHandler.text);
                    variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
                    Instantiate(variableForPrefab, new Vector3(0, 2f, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d").transform);
                }
        }
    }

    
}