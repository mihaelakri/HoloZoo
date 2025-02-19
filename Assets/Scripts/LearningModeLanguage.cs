using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearningModeLanguage : MonoBehaviour
{
    public Text intro_cm;

    // Mode buttons
    public Text globus_mode;
    public Text list_mode; 

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                {"intro_cm", "Choose a way to learn about animals. "},
                { "globus_mode", "GLOBUS MODE" },
                { "list_mode", "LIST MODE" },
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                {"intro_cm", "Choisissez un moyen d'apprendre sur les animaux."},
                { "globus_mode", "MODE GLOBUS" },
                { "list_mode", "MODE LISTE" },
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                {"intro_cm", "Odaberi način na koji ćeš učiti o životinjama."},
                { "globus_mode", "GLOBUS" },
                { "list_mode", "POPIS" },
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                {"intro_cm", "Elige una forma de aprender sobre los animales."},
                { "globus_mode", "MODO GLOBUS" },
                { "list_mode", "MODO LISTA" },
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                {"intro_cm", "Válassz egy módot az állatok megismerésére."},
                { "globus_mode", "GLOBUS MÓD" },
                { "list_mode", "LISTA MÓD" },
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

            intro_cm.text = languageTexts["intro_cm"];
            globus_mode.text = languageTexts["globus_mode"];
            list_mode.text = languageTexts["list_mode"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
