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

        if (!PlayerPrefs.HasKey("ID"))
        {
            SceneManager.LoadScene(sceneNameToLoad);
            yield break;
        }

        if (PlayerPrefs.GetString("device") == "mobile")
            SceneManager.LoadScene("Home");
        else
            SceneManager.LoadScene("HologramTablet");
    }
}