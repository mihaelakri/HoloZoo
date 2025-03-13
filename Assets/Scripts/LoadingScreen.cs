using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private float delayBeforeLoading = 5f;
    [SerializeField]
    private string sceneNameToLoad = "Language";

    void Start()
    {
        StartCoroutine(SceneSwitch());
    }

    IEnumerator SceneSwitch()
    {
        yield return new WaitForSeconds(delayBeforeLoading);
        yield return new WaitUntil(() => 
            GameData.isMainDataLoaded && GameData.isUserDataLoaded
        );
        
        if (PlayerPrefs.GetString("device") == "tablet")
            SceneManager.LoadScene("HologramTablet");

        else if (!PlayerPrefs.HasKey("ID"))
            SceneManager.LoadScene(sceneNameToLoad);

        else if (PlayerPrefs.GetString("device") == "mobile")
            SceneManager.LoadScene("Home");
    }
}