using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Dropdown : MonoBehaviour
{
    public GameObject dropdown;
    public GameObject dropdownItem;
    public Text text;
    public float y;
    public int clickedTag;

   public void pushDown(){
        if (Convert.ToInt16(dropdown.tag) == 0){
            float y_clicked = dropdown.transform.position.y;
            float x_clicked = dropdown.transform.position.x;
            dropdownItem.transform.position = new Vector3(563,-255,0);
            dropdown.tag = clickedTag.ToString();
            foreach (Transform child  in transform.parent.gameObject.transform){
                if(Convert.ToInt16(child.tag) > clickedTag){
                //move maindropdown
                y = child.transform.localPosition.y + 67;
                child.transform.localPosition = new Vector3(0, y, 0);
                }
            }
            dropdown.transform.GetChild(0).transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
            text.color = Color.black;
        }else{
            clickedTag = Convert.ToInt16(dropdown.tag);
            float y_clicked = dropdown.transform.position.y;
            float x_clicked = dropdown.transform.position.x;
            foreach (Transform child  in transform.parent.gameObject.transform){
                if(Convert.ToInt16(child.tag) > clickedTag){
                //move maindropdown
                y = child.transform.localPosition.y - 67;
                child.transform.localPosition = new Vector3(0, y, 0);
                }
            }
            //move dropdown
            dropdownItem.transform.position = new Vector3(x_clicked, y_clicked, 0);
            dropdownItem.transform.LeanMove(new Vector3(x_clicked,y_clicked-77,0),1).setEaseOutQuart();
            text.color = Color.white;
            dropdown.transform.GetChild(0).transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
            dropdown.tag = "0";
        }
   }
}
