using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ApplyAccessibilityCM : MonoBehaviour
{

    public Font liberationFont;
    public Font jostFont;

    Text[] textComponents;
    void Start()
    {
        // contrast
        if(PlayerPrefs.GetInt("contrast")==1){
            GameObject.Find("btn1").GetComponent<Image>().color = Color.black; 
            GameObject.Find("btn2").GetComponent<Image>().color = Color.black; 
        }else{
            GameObject.Find("btn1").GetComponent<Image>().color = Color.white; 
            GameObject.Find("btn2").GetComponent<Image>().color = Color.white; 
        }
        // dyslexia
        textComponents = FindObjectsOfType<Text>();
        if(PlayerPrefs.GetInt("dyslexia")==1){
            foreach (Text textComponent in textComponents)
            {
                textComponent.font = liberationFont;
            }
        }else{
            foreach (Text textComponent in textComponents)
            {
                textComponent.font = jostFont;
            }
        }

    }

}
