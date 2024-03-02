using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class HomeScreen : MonoBehaviour
{
    public int id; 
    [SerializeField] public Profile_info profile_info; 


    [Serializable]
    public class Profile_info{
        public String username;
        public int experience;
        public int level;
    }
    
    // Start is called before the first fram
    void Start()
    {
        if(PlayerPrefs.GetInt("contrast")==1){
            GameObject.Find("Bar").GetComponent<Image>().color = Color.black; 
            GameObject.Find("btn1").GetComponent<Image>().color = Color.black; 
            GameObject.Find("btn2").GetComponent<Image>().color = Color.black; 
        }else{
            GameObject.Find("Bar").GetComponent<Image>().color = Color.white; 
            GameObject.Find("btn1").GetComponent<Image>().color = Color.white; 
            GameObject.Find("btn2").GetComponent<Image>().color = Color.white; 
        }

        if(PlayerPrefs.GetInt("textToSpeech")==1){
            GameObject.Find("TTSHelper").GetComponent<TtsGlobal>().readHome();
        }
        
    }

}
