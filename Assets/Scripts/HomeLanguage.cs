using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeLanguage : MonoBehaviour
{
    // home buttons
    public Text learn; 
    public Text quiz;
    //accessibility tekst
    public Text font_size; 
    public Text dyslexia; 
    public Text contrast_text; 
    public Text tts_text; 
    public Text save_btn; 

    public Text small; 
    public Text medium; 
    public Text large; 

    // Language
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                { "learn", "LEARN" },
                { "quiz", "QUIZ" },
                { "font_size", "Font size"},
                { "dyslexia", "Dyslexia font" },
                { "contrast_text", "Contrast" },
                { "tts_text", "Text to speech" },
                { "small", "small" },
                { "medium", "medium" },
                { "large", "large" },
                { "save_btn", "SAVE" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "learn", "APPRENDRE" },
                { "quiz", "QUIZ" },
                { "font_size", "Taille de la police"},
                { "dyslexia", "police Dyslexie" },
                { "contrast_text", "Contraste" },
                { "tts_text", "Synthèse vocale" },
                { "small", "petit" },
                { "medium", "moyen" },
                { "large", "grand" },
                { "save_btn", "ENREGISTRER" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "learn", "UČENJE" },
                { "quiz", "KVIZ" },
                { "font_size", "Veličina fonta"},
                { "dyslexia", "Font za disleksiju" },
                { "contrast_text", "Kontrast" },
                { "tts_text", "Tekst u govor" },
                { "small", "mali" },
                { "medium", "srednji" },
                { "large", "veliki" },
                { "save_btn", "SPREMI" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "learn", "APRENDER" },
                { "quiz", "CUESTIONARIO" },
                { "font_size", "Tamaño de fuente"},
                { "dyslexia", "Fuente para dislexia" },
                { "contrast_text", "Contraste" },
                { "tts_text", "Iniciar" },
                { "small", "pequeño" },
                { "medium", "mediano" },
                { "large", "grande" },
                { "save_btn", "GUARDAR" }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "learn", "TANUL" },
                { "quiz", "KVÍZ" },
                { "font_size", "Betűméret"},
                { "dyslexia", "Dyslexia font" },
                { "contrast_text", "Diszlexia betűtípus" },
                { "tts_text", "Szöveg beszédté" },
                { "small", "kicsi" },
                { "medium", "közepes" },
                { "large", "nagy" },
                { "save_btn", "MEGTAKARÍTÁS" }
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

            learn.text = languageTexts["learn"];
            quiz.text = languageTexts["quiz"];
            font_size.text = languageTexts["font_size"];
            contrast_text.text = languageTexts["contrast_text"];
            tts_text.text = languageTexts["tts_text"];
            dyslexia.text = languageTexts["dyslexia"];
            save_btn.text = languageTexts["save_btn"];
            small.text = languageTexts["small"];
            medium.text = languageTexts["medium"];
            large.text = languageTexts["large"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
