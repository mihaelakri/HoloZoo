using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearningModeLanguage : MonoBehaviour
{
     // Accessibility 
    public Text font_size; 
    public Text dyslexia; 
    public Text contrast_text; 
    public Text tts_text; 
    public Text save_btn; 

    // Mode buttons
    public Text globus_mode;
    public Text list_mode; 

    // Language
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {

        {
            "en", new Dictionary<string, string>
            {
                { "globus_mode", "GLOBUS MODE" },
                { "list_mode", "LIST MODE" },
                { "font_size", "Font size"},
                { "dyslexia", "Dyslexia font" },
                { "contrast_text", "Contrast" },
                { "tts_text", "Text to speech" },
                { "save_btn", "SAVE" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "globus_mode", "MODE GLOBUS" },
                { "list_mode", "MODE LISTE" },
                { "font_size", "Taille de la police"},
                { "dyslexia", "police Dyslexie" },
                { "contrast_text", "Contraste" },
                { "tts_text", "Synthèse vocale" },
                { "save_btn", "ENREGISTRER" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "globus_mode", "GLOBUS" },
                { "list_mode", "POPIS" },
                { "font_size", "Veličina fonta"},
                { "dyslexia", "Font za disleksiju" },
                { "contrast_text", "Kontrast" },
                { "tts_text", "Tekst u govor" },
                { "save_btn", "SPREMI" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "globus_mode", "MODO GLOBUS" },
                { "list_mode", "MODO LISTA" },
                { "font_size", "Tamaño de fuente"},
                { "dyslexia", "Fuente para dislexia" },
                { "contrast_text", "Contraste" },
                { "tts_text", "Iniciar" },
                { "save_btn", "GUARDAR" }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "globus_mode", "GLOBUS MÓD" },
                { "list_mode", "LISTA MÓD" },
                { "font_size", "Betűméret"},
                { "dyslexia", "Dyslexia font" },
                { "contrast_text", "Diszlexia betűtípus" },
                { "tts_text", "Szöveg beszédté" },
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

            globus_mode.text = languageTexts["globus_mode"];
            list_mode.text = languageTexts["list_mode"];
            font_size.text = languageTexts["font_size"];
            contrast_text.text = languageTexts["contrast_text"];
            tts_text.text = languageTexts["tts_text"];
            dyslexia.text = languageTexts["dyslexia"];
            save_btn.text = languageTexts["save_btn"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
