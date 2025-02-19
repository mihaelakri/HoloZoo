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
    public GameObject parentObject;
    public GameObject alternateObject;
    public Text text;
    public float y;
    public int clickedTag;

    public void pushDown()
    {
        //vrati ga
        if (Convert.ToInt16(dropdown.tag) == 0)
        {
            dropdownItem.transform.SetParent(alternateObject.transform, true);
            // take coordinates of clicked dropdown
            float y_clicked = dropdown.transform.position.y;
            float x_clicked = dropdown.transform.position.x;
            // sto je ovo
            dropdownItem.transform.position = new Vector3(563, -255, 0);
            // vraca originalni tag
            dropdown.tag = clickedTag.ToString();
            // pomice sve ostale dole
            foreach (Transform child in transform.parent.gameObject.transform)
            {
                if (Convert.ToInt16(child.tag) > clickedTag)
                {
                    //move maindropdown
                    y = child.transform.localPosition.y + 67;
                    child.transform.localPosition = new Vector3(0, y, 0);
                }
            }

            // okrece strelicu
            dropdown.transform.GetChild(0).transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);

        }
        else
        {
            //spusti ga
            clickedTag = Convert.ToInt16(dropdown.tag);
            dropdownItem.transform.SetParent(parentObject.transform, true);
            dropdownItem.transform.SetSiblingIndex(dropdown.transform.GetSiblingIndex() + 1);
            float y_clicked = dropdown.transform.position.y;
            float x_clicked = dropdown.transform.position.x;
            foreach (Transform child in transform.parent.gameObject.transform)
            {
                if (Convert.ToInt16(child.tag) > clickedTag)
                {
                    //move maindropdown
                    y = child.transform.localPosition.y - 67;
                    child.transform.localPosition = new Vector3(0, y, 0);
                }
            }
            //move dropdown
            //dropdownItem.transform.position = new Vector3(x_clicked, y_clicked, 0);
            //dropdownItem.transform.LeanMove(new Vector3(x_clicked,y_clicked-67,0),1).setEaseOutQuart();
            dropdown.transform.GetChild(0).transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
            dropdown.tag = "0";
        }
    }
}
