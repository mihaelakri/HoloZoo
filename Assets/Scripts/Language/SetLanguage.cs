using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetLanguage : MonoBehaviour
{
    public Button buttonCroatian;
    public Button buttonEnglish;
    public Button buttonFrench;
    public Button buttonHungarian;
    public Button buttonSpanish;

    private bool isLanguageSet = false;

    void Start()
    {
        // listeneri za svaki gumb
        buttonCroatian.onClick.AddListener(() => SetLanguageAndProceed("hr"));
        buttonEnglish.onClick.AddListener(() => SetLanguageAndProceed("en"));
        buttonFrench.onClick.AddListener(() => SetLanguageAndProceed("fr"));
        buttonHungarian.onClick.AddListener(() => SetLanguageAndProceed("hu"));
        buttonSpanish.onClick.AddListener(() => SetLanguageAndProceed("es"));
    }

    void SetLanguageAndProceed(string languageCode)
    {
        if (isLanguageSet)
        {
            Debug.Log($"{nameof(SetLanguage)} - Language already set");
            return;
        }

        // Avoids multiple button presses
        isLanguageSet = true;

        // Spremanje jezika lokalno u PlayerPrefs
        PlayerPrefs.SetString("lang", languageCode);
        PlayerPrefs.Save();
        GameData.Instance.LoadTranslatedTables(languageCode);
        Debug.Log($"{nameof(SetLanguage)} - Language successfully set");

        // Nakon slanja, učitavanje sljedeće scene
        SceneManager.LoadScene("Welcome");
    }
}