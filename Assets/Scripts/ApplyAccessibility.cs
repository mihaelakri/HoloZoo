using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ApplyAccessibility : MonoBehaviour
{
    public Font liberationFont;
    public Font jostFont;
    Text[] yourTexts;
    Text[] textComponents;
    // Start is called before the first frame update
    void Start()
    {
        //font size
        yourTexts = FindObjectsOfType<Text>();
        
        foreach (Text text in yourTexts)
        {
            Debug.Log(text);
            text.fontSize = PlayerPrefs.GetInt("font_size");
        }
        //contrast
        if(PlayerPrefs.GetInt("contrast")==1){
            GameObject.Find("saveBtn").GetComponent<Image>().color = Color.black;
            GameObject.Find("Scrollbar").GetComponent<Image>().color = Color.black;
            textComponents = FindObjectsOfType<Text>();
            
            foreach (Text textComponent in textComponents)
            {
                if(textComponent.text != "LEARN" && textComponent.text != "QUIZ" && textComponent.text != "SAVE"){
                    textComponent.color =  Color.black;
                }
            }

        }
        //dyslexia
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

    public void applyAcc(){
        Start();
    }
}
