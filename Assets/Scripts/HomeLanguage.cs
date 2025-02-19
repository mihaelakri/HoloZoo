using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeLanguage : MonoBehaviour
{
    // home buttons
    public Text learn; 
    public Text quiz;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                { "learn", "LEARN" },
                { "quiz", "QUIZ" },
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "learn", "APPRENDRE" },
                { "quiz", "QUIZ" },
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "learn", "UČENJE" },
                { "quiz", "KVIZ" },
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "learn", "APRENDER" },
                { "quiz", "CUESTIONARIO" },
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "learn", "TANUL" },
                { "quiz", "KVÍZ" },
            }
        }
    }; 

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        string selectedLanguage = PlayerPrefs.GetString("lang", "en");

        if (translations.ContainsKey(selectedLanguage))
        {
            var languageTexts = translations[selectedLanguage];

            learn.text = languageTexts["learn"];
            quiz.text = languageTexts["quiz"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
