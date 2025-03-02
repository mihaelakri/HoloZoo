using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneChangeTag : MonoBehaviour
{
    public void ChangeScene(string sceneNameToLoad)
    {
        PlayerPrefs.SetString("id_animal", EventSystem.current.currentSelectedGameObject.name);
        CommConstants.animal_id = int.Parse(PlayerPrefs.GetString("id_animal"));
        SceneManager.LoadScene(sceneNameToLoad);
    }
}