using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class RotateModel : MonoBehaviour
{

    public Slider bottomSlider;
    public Slider sideSlider;
    public GameObject model;
    public GameObject target;
    public float x,y;
    public float sliderLastX=0, sliderLastY=0;

    public void rotateModel(){
        Quaternion localRotation = Quaternion.Euler(sideSlider.value,bottomSlider.value, 0f);
        model.transform.GetChild(0).transform.rotation = transform.rotation * localRotation;
       
        StartCoroutine(Rotate3DModel());
    }

    IEnumerator Rotate3DModel(){
        Debug.Log(PlayerPrefs.GetString("id_animal"));
        WWWForm form = new WWWForm();
        form.AddField("x", sideSlider.value.ToString());
        form.AddField("y", bottomSlider.value.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/HoloZoo/rotate.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                     Debug.Log(www.downloadHandler.text);
                }
        }
    }
}
