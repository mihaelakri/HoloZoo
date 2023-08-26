using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Accessibility : MonoBehaviour
{
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
                PlayerPrefs.SetInt("font_size", 17);
            }else{
                PlayerPrefs.SetInt("font_size", 18);
            }
            Debug.Log(PlayerPrefs.GetInt("font_size"));
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

}
