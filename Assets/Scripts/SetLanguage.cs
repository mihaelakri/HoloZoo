using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SetLanguage : MonoBehaviour
{
    public Button buttonEnglish;
    public Button buttonFrench;
    public Button buttonCroatian;
    public Button buttonSpanish;
    public Button buttonHungarian;

    private bool isLanguageSet = false;

    void Start()
    {
        // listeneri za svaki gumb
        buttonEnglish.onClick.AddListener(() => StartCoroutine(SetLanguageAndProceed("en")));
        buttonFrench.onClick.AddListener(() => StartCoroutine(SetLanguageAndProceed("fr")));
        buttonCroatian.onClick.AddListener(() => StartCoroutine(SetLanguageAndProceed("hr")));
        buttonSpanish.onClick.AddListener(() => StartCoroutine(SetLanguageAndProceed("es")));
        buttonHungarian.onClick.AddListener(() => StartCoroutine(SetLanguageAndProceed("hu")));
    }

    IEnumerator SetLanguageAndProceed(string languageCode)
    {
        if (!isLanguageSet)
        {
            // Avoids multiple button presses
            isLanguageSet = true;

            // Spremanje jezika lokalno u PlayerPrefs
            PlayerPrefs.SetString("lang", languageCode);
            PlayerPrefs.Save();

            // Block while there's no internet
            yield return StartCoroutine(CheckInternetConnection.PromptConnectionBlocking(this));

            // Priprema podataka za slanje
            WWWForm form = new WWWForm();
            form.AddField("lang", languageCode);

            using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "middle_man.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error sending language: " + www.error);
                }
                else
                {
                    Debug.Log("Language sent successfully: " + languageCode);
                }
            }

            // Nakon slanja, učitavanje sljedeće scene
            SceneManager.LoadScene("Welcome");
        }
    }
}
