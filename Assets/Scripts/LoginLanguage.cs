using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginLanguage : MonoBehaviour
{
    //Login form
    public Text login_heading; 
    public Text username_placeholder; 
    public Text password_placeholder; 
    public Text register_btn; 
    public Text login_btn; 

    // Language
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                { "login_heading", "Log in" },
                { "username_placeholder", "Username" },
                { "password_placeholder", "Password"},
                { "register_btn", "REGISTER" },
                { "login_btn", "LOG IN" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "login_heading", "Se connecter" },
                { "username_placeholder", "Nom d'utilisateur" },
                { "password_placeholder", "Mot de passe"},
                { "register_btn", "CRÉER UN COMPTE" },
                { "login_btn", "SE CONNECTER" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "login_heading", "Prijava" },
                { "username_placeholder", "Korisničko ime" },
                { "password_placeholder", "Lozinka"},
                { "register_btn", "REGISTRACIJA" },
                { "login_btn", "PRIJAVI SE" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "login_heading", "Iniciar sesión" },
                { "username_placeholder", "Nombre de usuario" },
                { "password_placeholder", "Contraseña"},
                { "register_btn", "REGISTRO" },
                { "login_btn", "INICIAR SESIÓN" }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "login_heading", "Bejelentkezés" },
                { "username_placeholder", "Felhasználónév" },
                { "password_placeholder", "Jelszó"},
                { "register_btn", "REGISZTRÁCIÓ" },
                { "login_btn", "BEJELENTKEZÉS" }
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

            login_heading.text = languageTexts["login_heading"];
            username_placeholder.text = languageTexts["username_placeholder"];
            password_placeholder.text = languageTexts["password_placeholder"];
            register_btn.text = languageTexts["register_btn"];
            login_btn.text = languageTexts["login_btn"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
