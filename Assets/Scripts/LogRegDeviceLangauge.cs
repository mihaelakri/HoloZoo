using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogRegDeviceLangauge : MonoBehaviour
{
   //Pop up window (Choose device) 
    public Text choose_device; 
    public Text mobile; 
    public Text tablet; 

    // Buttons (log in/ register)

    public Text login; 
    public Text register; 

    // Language
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                { "choose_device", "Choose device purpose" }, // Context change for choosing device purpose
                { "game", "Game" }, // "Mobile" changed to "Game"
                { "hologram", "Hologram" }, // "Tablet" changed to "Hologram"
                { "login", "LOG IN" },
                { "register", "REGISTER" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                 { "choose_device", "Choisir le but de l'appareil" }, // Context change for choosing device purpose
                { "game", "Jeu" }, // "Smartphone" changed to "Game"
                { "hologram", "Hologramme" }, // "Tablet" changed to "Hologram"
                { "login", "SE CONNECTER" },
                { "register", "CRÉER UN COMPTE" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "choose_device", "Odaberi svrhu uređaja" }, // Context change for choosing device purpose
                { "game", "Igra" }, // "Mobilni" changed to "Igra"
                { "hologram", "Hologram" }, // "Tablet" changed to "Hologram"
                { "login", "PRIJAVA" },
                { "register", "REGISTRACIJA" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "choose_device", "Elige el propósito del dispositivo" }, // Context change for choosing device purpose
                { "game", "Juego" }, // "Teléfono móvil" changed to "Juego"
                { "hologram", "Holograma" }, // "Tableta" changed to "Holograma"
                { "login", "INICIAR SESIÓN" },
                { "register", "REGISTRO" }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "choose_device", "Válaszd ki az eszköz célját" }, // Context change for choosing device purpose
                { "game", "Játék" }, // "Okostelefon" changed to "Játék"
                { "hologram", "Hologram" }, // "Tablet" changed to "Hologram"
                { "login", "BEJELENTKEZÉS" },
                { "register", "REGISZTRÁCIÓ" }
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

            choose_device.text = languageTexts["choose_device"];
            mobile.text = languageTexts["game"];
            tablet.text = languageTexts["hologram"];
            login.text = languageTexts["login"];
            register.text = languageTexts["register"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
