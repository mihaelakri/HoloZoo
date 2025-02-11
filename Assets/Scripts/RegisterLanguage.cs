using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterLanguage : MonoBehaviour
{
    // Register Form
    public Text register_heading; 
    public Text username_placeholder; 
    public Text password_placeholder; 
    public Text password_repeat_placeholder; 
    public Text register_btn; 

    // Language
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                { "register_heading", "Register" },
                { "username_placeholder", "Username" },
                { "password_placeholder", "Password" },
                { "password_repeat_placeholder", "Repeat password"},
                { "register_btn", "REGISTER" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "register_heading", "Créer un compte" },
                { "username_placeholder", "Nom d'utilisateur" },
                { "password_placeholder", "Mot de passe" },
                { "password_repeat_placeholder", "Confirmer le mot de passe"},
                { "register_btn", "CRÉER UN COMPTE" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "register_heading", "Registracija" },
                { "username_placeholder", "Korisničko ime" },
                { "password_placeholder", "Lozinka" },
                { "password_repeat_placeholder", "Ponovi lozinku"},
                { "register_btn", "REGIST" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "register_heading", "Registro" },
                { "username_placeholder", "Nombre de usuario" },
                { "password_placeholder", "Contraseña" },
                { "password_repeat_placeholder", "Repite la contraseña"},
                { "register_btn", "REGISTRO" }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "register_heading", "Regisztráció" },
                { "username_placeholder", "Felhasználónév" },
                { "password_placeholder", "Jelszó" },
                { "password_repeat_placeholder", "Jelszó újra"},
                { "register_btn", "REGISZTRÁCIÓ" }
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

            register_heading.text = languageTexts["register_heading"];
            username_placeholder.text = languageTexts["username_placeholder"];
            password_placeholder.text = languageTexts["password_placeholder"];
            password_repeat_placeholder.text = languageTexts["password_repeat_placeholder"];
            register_btn.text = languageTexts["register_btn"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
