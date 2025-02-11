using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizScoreLanguage : MonoBehaviour
{
    // Quiz Score Buttons
    public Text home_btn; 
    public Text new_quiz_btn; 

    // Language
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                { "home_btn", "Home" },
                { "new_quiz_btn", "New Quiz" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "home_btn", "Accueil" },
                { "new_quiz_btn", "Commencer un autre quiz" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "home_btn", "Početak" },
                { "new_quiz_btn", "Novi kviz" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "home_btn", "Inicio" },    
                { "new_quiz_btn", "Empieza un nuevo test" }    
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "home_btn", "Kezdőképernyő" },
                { "new_quiz_btn", "Új kvíz indítása" }
            }
        }
    }; 

    void Start()
    {
        selectedLanguage = PlayerPrefs.GetString("lang", "en");
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        if (translations.ContainsKey(selectedLanguage))
        {
            var languageTexts = translations[selectedLanguage];

            home_btn.text = languageTexts["home_btn"];
            new_quiz_btn.text = languageTexts["new_quiz_btn"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}

