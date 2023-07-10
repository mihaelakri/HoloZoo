using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenAccesibility : MonoBehaviour
{
    public GameObject panel;
    public Scrollbar fontSize;
    public Toggle dyslexiaToggle;
    public Toggle ttsToggle;
    public Toggle contrastToggle;
    
    public void showAccesibility(){
        if(PlayerPrefs.GetInt("font_size")==16){
            fontSize.value = 0;
        }else if(PlayerPrefs.GetInt("font_size")==17){
            fontSize.value = 0.5f;
        }else{
            fontSize.value = 1;
        }
        if (PlayerPrefs.GetInt("dyslexia")==1) dyslexiaToggle.isOn = true;
        if (PlayerPrefs.GetInt("contrast")==1) contrastToggle.isOn = true;
        if (PlayerPrefs.GetInt("tts")==1) ttsToggle.isOn = true;
        panel.transform.LeanMoveLocal(new Vector2(0,0),1).setEaseOutQuart();
    }
    public void hideAccesibility(){
        panel.transform.LeanMoveLocal(new Vector2(0,-645),1).setEaseOutQuart();
        if(PlayerPrefs.GetInt("contrast")==1){
            GameObject.Find("Bar").GetComponent<Image>().color = Color.black; 
            GameObject.Find("btn1").GetComponent<Image>().color = Color.black; 
            GameObject.Find("btn2").GetComponent<Image>().color = Color.black; 
        }else{
            GameObject.Find("Bar").GetComponent<Image>().color = Color.white; 
            GameObject.Find("btn1").GetComponent<Image>().color = Color.white; 
            GameObject.Find("btn2").GetComponent<Image>().color = Color.white; 
        }
    }
}
