using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetDifficulty : MonoBehaviour
{
    public Scrollbar difficulty; 
    public Button submitButton;

    Text[] yourTexts;

    void Start()
    {
        yourTexts = FindObjectsOfType<Text>();
        foreach (Text text in yourTexts)
        {
            if(text.fontSize < 18){
                text.fontSize = PlayerPrefs.GetInt("font_size");
            }
        }
        if(PlayerPrefs.GetInt("contrast")==1){
            GameObject.Find("btn").GetComponent<Image>().color = Color.black;
            GameObject.Find("Scrollbar").GetComponent<Image>().color = Color.black;
        }
    }

    public void SetDiff(){
        float diff = difficulty.value;
        if(diff < 0.5f){
            Setint(1);
        }else if(diff >= 0.5f && diff < 1){
            Setint(2);
        }else{
            Setint(3);
        }
        Debug.Log(Getint("diff"));
    }

    public void Setint(int Value)
    {
        PlayerPrefs.SetInt("diff", Value);
    }

    public int Getint(string KeyName)
    {
        return PlayerPrefs.GetInt(KeyName);
    }
}
