using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneChangeTag : MonoBehaviour
{
public void ChangeScene(string sceneNameToLoad) {  
        PlayerPrefs.SetString("id_animal", EventSystem.current.currentSelectedGameObject.name);
        CommConstants.animal_id = int.Parse(PlayerPrefs.GetString("id_animal"));
        SceneManager.LoadScene(sceneNameToLoad);  
    } 
}
