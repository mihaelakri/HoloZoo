using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneGlobe : MonoBehaviour
{
    public string id; 

    public void ChangeScene(string sceneNameToLoad) {  
        PlayerPrefs.SetString("id_animal", id);
        SceneManager.LoadScene(sceneNameToLoad);  
    } 
}
