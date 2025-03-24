using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SetLanguage : MonoBehaviour
{
    [SerializeField]
    Button buttonCroatian, buttonEnglish, buttonFrench, buttonHungarian, buttonSpanish;
    bool isLanguageSet = false;

    void Start()
    {
        // listeneri za svaki gumb
        buttonCroatian.onClick.AddListener(() => StartCoroutine(SetLanguageAndProceed("hr")));
        buttonEnglish.onClick.AddListener(() => StartCoroutine(SetLanguageAndProceed("en")));
        buttonFrench.onClick.AddListener(() => StartCoroutine(SetLanguageAndProceed("fr")));
        buttonHungarian.onClick.AddListener(() => StartCoroutine(SetLanguageAndProceed("hu")));
        buttonSpanish.onClick.AddListener(() => StartCoroutine(SetLanguageAndProceed("es")));
    }

    IEnumerator SetLanguageAndProceed(string languageCode)
    {
        if (isLanguageSet)
        {
            Debug.Log($"{nameof(SetLanguage)} - Language already set");
            yield break;
        }

        // Avoids multiple button presses
        isLanguageSet = true;

        // Spremanje jezika lokalno u PlayerPrefs
        PlayerPrefs.SetString("lang", languageCode);
        PlayerPrefs.Save();
        yield return StartCoroutine(GameData.Instance.LoadTranslatedTables(languageCode));
        Debug.Log($"{nameof(SetLanguage)} - Language successfully set");

        // Nakon slanja, učitavanje sljedeće scene
        SceneManager.LoadScene("Welcome");
    }
}