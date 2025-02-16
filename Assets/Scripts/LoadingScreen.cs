using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LoadingScreen : MonoBehaviour
{
    public int id;
    [SerializeField]
    private float delayBeforeLoading = 5f;
    [SerializeField]
    private string sceneNameToLoad = "Language";
    private bool isSessionSet;

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

        yield return StartCoroutine(CheckInternetConnection.PromptConnectionBlocking(this));
        yield return StartCoroutine(SetSession());

        if (isSessionSet == true)
        {
            id = Getint("ID");
            if (PlayerPrefs.GetString("device") == "mobile")
            {
                SceneManager.LoadScene("Home");
            }
            else
            {
                SceneManager.LoadScene("HologramTablet");
            }
        }
        else
        {
            // fallback in case of remote server error
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }

    IEnumerator SetSession()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("lang", PlayerPrefs.GetString("lang", "en"));
        form.AddField("flag", "5");

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "middle_man.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("set session error: " + www.error);
                isSessionSet = false;
            }
            else
            {
                Debug.Log("set session: " + www.downloadHandler.text);
                isSessionSet = true;
            }
        }
    }

    public int Getint(string KeyName)
    {
        return PlayerPrefs.GetInt(KeyName);
    }
}
