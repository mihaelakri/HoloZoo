using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileLanguage : MonoBehaviour
{
    //txts
    public Text level; 
    public Text experience; 
    public Text new_password; 
    public Text reapeat_password; 

    // Button
    public Text confirm; 

    // Language
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                { "level", "Level" },
                { "experience", "Experience" },
                { "new_password", "New password" },
                { "reapeat_password", "Retype password" },
                { "confirm", "Confirm" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "level", "Niveau" },
                { "experience", "Expérience" },
                { "new_password", "Entrer un nouveau mot de passe" },
                { "reapeat_password", "Confirmer le nouveau mot de passe" },
                { "confirm", "Confirmer" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "level", "Level" },
                { "experience", "Bodovi" },
                { "new_password", "Unesi novu lozinku" },
                { "reapeat_password", "Ponovi novu lozinku" },
                { "confirm", "Potvrdi" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "level", "Nivel" },
                { "experience", "Experiencia" },
                { "new_password", "Entra una nueva contraseña" },
                { "reapeat_password", "Reita la nueva contraseña" },
                { "confirm", "Confirmar" }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "level", "Szint" },
                { "experience", "Tapasztalat" },
                { "new_password", "Új jelszó megadása" },
                { "reapeat_password", "Új jelszó megismétlése" },
                { "confirm", "Megerősítés" }
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

            level.text = languageTexts["level"];
            experience.text = languageTexts["experience"];
            new_password.text = languageTexts["new_password"];
            reapeat_password.text = languageTexts["reapeat_password"];
            confirm.text = languageTexts["confirm"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
