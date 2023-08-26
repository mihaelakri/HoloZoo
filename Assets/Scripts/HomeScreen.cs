using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    // Start is called before the first frame update
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
    }

}
