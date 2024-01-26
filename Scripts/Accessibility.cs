using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Accessibility : MonoBehaviour
{   
    public GameObject panel;
    public Scrollbar fontSizeScrollBar; 
    public Toggle dyslexiaToggle;
    public Toggle contrastToggle;
    public Toggle textToSpeechToggle;
    public Text dyslexiaText;
    public Text contrastText;
    public Text ttsText;
    
    public void SetFontSize(){
            float fontSize= fontSizeScrollBar.value;
            if(fontSize < 0.5f){
                PlayerPrefs.SetInt("font_size", 16);
            }else if(fontSize >= 0.5f && fontSize < 0.8){
                PlayerPrefs.SetInt("font_size", 18);
            }else{
                PlayerPrefs.SetInt("font_size", 20);
            }
    }

    public void SetDyslexia(){
        if(dyslexiaToggle.isOn){
            dyslexiaText.text = "On";
            PlayerPrefs.SetInt("dyslexia", 1);
        } else{
            PlayerPrefs.SetInt("dyslexia", 0);
            dyslexiaText.text = "Off";

        }
    }
    public void SetContrast(){
        if(contrastToggle.isOn){
            PlayerPrefs.SetInt("contrast", 1);
            contrastText.text = "On";
        } else{
            PlayerPrefs.SetInt("contrast", 0);
            contrastText.text = "Off";
        }
    }
    public void SetTextToSpeech(){
        if(textToSpeechToggle.isOn){
            PlayerPrefs.SetInt("textToSpeech", 1);
            ttsText.text = "On";
        } else{
            PlayerPrefs.SetInt("textToSpeech", 0);
            ttsText.text = "Off";
        }
    }

    public void showAccesibility(){
        if(PlayerPrefs.GetInt("font_size")==16){
            fontSizeScrollBar.value = 0;
        }else if(PlayerPrefs.GetInt("font_size")==17){
            fontSizeScrollBar.value = 0.5f;
        }else{
            fontSizeScrollBar.value = 1;
        }
        if (PlayerPrefs.GetInt("dyslexia")==1) dyslexiaToggle.isOn = true;
        if (PlayerPrefs.GetInt("contrast")==1) contrastToggle.isOn = true;
        if (PlayerPrefs.GetInt("textToSpeech")==1) textToSpeechToggle.isOn = true;
        panel.transform.LeanMoveLocal(new Vector2(0,0),1).setEaseOutQuart();
    }
    public void hideAccesibility(){
        panel.transform.LeanMoveLocal(new Vector2(0,-645),1).setEaseOutQuart();
        if(PlayerPrefs.GetInt("contrast")==1){
            
            if(GameObject.Find("Bar") != null){
                GameObject.Find("Bar").GetComponent<Image>().color = Color.black;
            }
             
            GameObject.Find("btn1").GetComponent<Image>().color = Color.black; 
            GameObject.Find("btn2").GetComponent<Image>().color = Color.black; 
            GameObject.Find("saveBtn").GetComponent<Image>().color = Color.black;

        }else{
            if(GameObject.Find("Bar") != null){
                GameObject.Find("Bar").GetComponent<Image>().color = Color.white;
            }
            
            GameObject.Find("btn1").GetComponent<Image>().color = Color.white; 
            GameObject.Find("btn2").GetComponent<Image>().color = Color.white;
            GameObject.Find("saveBtn").GetComponent<Image>().color = Color.white;
            GameObject.Find("Scrollbar").GetComponent<Image>().color = Color.white;

        }
    }

}
