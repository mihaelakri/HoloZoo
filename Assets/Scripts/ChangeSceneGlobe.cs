using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneGlobe : MonoBehaviour
{
    public string id; 

    public void ChangeScene(string sceneNameToLoad) {  
        PlayerPrefs.SetString("id_animal", id);
        CommConstants.animal_id = int.Parse(PlayerPrefs.GetString("id_animal"));
        SceneManager.LoadScene(sceneNameToLoad);  
    } 
}
