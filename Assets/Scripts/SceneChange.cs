using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void ChangeScene(string sceneNameToLoad) {  
        if (sceneNameToLoad == "HologramGlobe") {
            PlayerPrefs.SetString("id_animal", "0");  // Loads globe model to display
            CommConstants.animal_id = 0;
        }
        SceneManager.LoadScene(sceneNameToLoad);  
    } 
}
