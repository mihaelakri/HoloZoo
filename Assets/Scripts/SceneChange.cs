using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string id;

    public void ChangeScene(string sceneNameToLoad)
    {
        if (sceneNameToLoad == "HologramGlobe")
        {
            PlayerPrefs.SetString("id_animal", "0");  // Loads globe model to display
            CommConstants.animal_id = 0;
        }
        SceneManager.LoadScene(sceneNameToLoad);
    }

    public void ChangeSceneToAnimalProfile(string sceneNameToLoad)
    {
        PlayerPrefs.SetString("id_animal", EventSystem.current.currentSelectedGameObject.name);
        CommConstants.animal_id = int.Parse(PlayerPrefs.GetString("id_animal"));
        SceneManager.LoadScene(sceneNameToLoad);
    }

    public void ChangeSceneGlobe(string sceneNameToLoad)
    {
        PlayerPrefs.SetString("id_animal", id);
        CommConstants.animal_id = int.Parse(PlayerPrefs.GetString("id_animal"));
        SceneManager.LoadScene(sceneNameToLoad);
    }

    public void GoBack()
    {
        SceneManagement.Instance.HandleBackButton();
    }
}