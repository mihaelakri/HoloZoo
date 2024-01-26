using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseDevice : MonoBehaviour
{
    public GameObject panel;
    public Toggle m_Toggle;

    public void chooseMobile(){
        storeCommMethod();
        PlayerPrefs.SetString("device","mobile");
        panel.transform.LeanMoveLocal(new Vector2(0,-645),1).setEaseOutQuart();
    }
     public void chooseTablet(){
        storeCommMethod();
        PlayerPrefs.SetString("device","tablet");
        panel.transform.LeanMoveLocal(new Vector2(0,-645),1).setEaseOutQuart();
    }

    private void storeCommMethod() {
        if (m_Toggle.isOn) {
            PlayerPrefs.SetString("conn_method","bluetooth");
        } else {
            PlayerPrefs.SetString("conn_method","websocket");
        }
    }
}
