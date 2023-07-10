using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseDevice : MonoBehaviour
{
    public GameObject panel;

    public void chooseMobile(){
        PlayerPrefs.SetString("device","mobile");
        panel.transform.LeanMoveLocal(new Vector2(0,-645),1).setEaseOutQuart();
    }
     public void chooseTablet(){
        PlayerPrefs.SetString("device","tablet");
        panel.transform.LeanMoveLocal(new Vector2(0,-645),1).setEaseOutQuart();
    }
}
