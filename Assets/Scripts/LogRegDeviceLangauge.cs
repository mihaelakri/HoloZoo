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
    public Text using_bluetooth; 

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
                { "choose_device", "Choose a device" },
                { "mobile", "Mobile" },
                { "tablet", "Tablet" },
                { "using_bluetooth", "Using Bluetooth?" },
                { "login", "LOG IN" },
                { "register", "REGISTER" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "choose_device", "Sélectionner le support" },
                { "mobile", "Smartphone" },
                { "tablet", "Tablette" },
                { "using_bluetooth", "Utiliser le mode bluetooth" },
                { "login", "SE CONNECTER" },
                { "register", "CRÉER UN COMPTE" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "choose_device", "Odaberi uređaj" },
                { "mobile", "Mobilni" },
                { "tablet", "Tablet" },
                { "using_bluetooth", "Koristite Bluetooth?" },
                { "login", "PRIJAVA" },
                { "register", "REGISTRACIJA" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "choose_device", "Elije el dispositivo" },
                { "mobile", "Teléfono móvil" },
                { "tablet", "Tableta" },
                { "using_bluetooth", "Usa bluetooth" },
                { "login", "INICIAR SESIÓN" },
                { "register", "REGISTRO" }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "choose_device", "Válaszd ki az eszközt" },
                { "mobile", "Okostelefon" },
                { "tablet", "Tablet" },
                { "using_bluetooth", "Bluetooth használat" },
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
            mobile.text = languageTexts["mobile"];
            tablet.text = languageTexts["tablet"];
            using_bluetooth.text = languageTexts["using_bluetooth"];
            login.text = languageTexts["login"];
            register.text = languageTexts["register"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
