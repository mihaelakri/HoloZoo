using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class QuizDiffLanguage : MonoBehaviour
{
    // Choose diffciulty 
    public Text choose_difficulty; 
    public Text easy; 
    public Text medium; 
    public Text hard; 
    public Text start_quiz_btn; 

    // Language
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
         {
            "en", new Dictionary<string, string>
            {
                { "choose_difficulty", "Choose Difficulty" },
                { "easy", "Easy" },
                { "medium", "Medium" },
                { "hard", "Hard" },
                { "start_quiz_btn", "Start Quiz" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "choose_difficulty", "Choisissez la difficulté" },
                { "easy", "Facile" },
                { "medium", "Moyen" },
                { "hard", "Difficile" },
                { "start_quiz_btn", "Commencer le quiz" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "choose_difficulty", "Odaberite težinu" },
                { "easy", "Lako" },
                { "medium", "Srednje" },
                { "hard", "Teško" },
                { "start_quiz_btn", "Započni kviz" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "choose_difficulty", "Elige dificultad" },
                { "easy", "Fácil" },
                { "medium", "Medio" },
                { "hard", "Difícil" },
                { "start_quiz_btn", "Comenzar quiz" }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "choose_difficulty", "Válaszd ki a kvíz nehézségi szintjé" },
                { "easy", "Könnyű" },
                { "medium", "Közepes" },
                { "hard", "Nehéz" },
                { "start_quiz_btn", "Indítsa el a kvízt" }
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

            choose_difficulty.text = languageTexts["choose_difficulty"];
            easy.text = languageTexts["easy"];
            medium.text = languageTexts["medium"];
            hard.text = languageTexts["hard"];
            start_quiz_btn.text = languageTexts["start_quiz_btn"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
