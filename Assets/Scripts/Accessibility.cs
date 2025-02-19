using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Accessibility : MonoBehaviour
{
    public Scrollbar fontSizeScrollBar;
    public Toggle dyslexiaToggle;
    public Toggle contrastToggle;
    public Toggle textToSpeechToggle;

    public Text font_size;
    public Text dyslexiaText;
    public Text dyslexiaStatusText;
    public Text contrastText;
    public Text contrastStatusText;
    public Text ttsText;
    public Text ttsStatusText;
    public Text save_btn;
    public Text small;
    public Text medium;
    public Text large;

    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
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
        ApplyLanguageTexts();
    }

    private void ApplyLanguageTexts()
    {
        string selectedLanguage = PlayerPrefs.GetString("lang", "en");

        var languageTexts = translations[selectedLanguage];

        font_size.text = languageTexts["font_size"];
        dyslexiaText.text = languageTexts["dyslexia"];
        contrastText.text = languageTexts["contrast_text"];
        ttsText.text = languageTexts["tts_text"];
        save_btn.text = languageTexts["save_btn"];
        small.text = languageTexts["small"];
        medium.text = languageTexts["medium"];
        large.text = languageTexts["large"];

    }

    public void SetFontSize()
    {
        float fontSize = fontSizeScrollBar.value;
        if (fontSize < 0.5f)
            PlayerPrefs.SetInt("font_size", 16);
        else if (fontSize >= 0.5f && fontSize < 0.8)
            PlayerPrefs.SetInt("font_size", 18);
        else
            PlayerPrefs.SetInt("font_size", 20);
    }

    public void SetDyslexia()
    {
        if (dyslexiaToggle.isOn)
        {
            dyslexiaStatusText.text = "On";
            PlayerPrefs.SetInt("dyslexia", 1);
        }
        else
        {
            PlayerPrefs.SetInt("dyslexia", 0);
            dyslexiaStatusText.text = "Off";
        }
    }

    public void SetContrast()
    {
        if (contrastToggle.isOn)
        {
            PlayerPrefs.SetInt("contrast", 1);
            contrastStatusText.text = "On";
        }
        else
        {
            PlayerPrefs.SetInt("contrast", 0);
            contrastStatusText.text = "Off";
        }
    }

    public void SetTextToSpeech()
    {
        if (textToSpeechToggle.isOn)
        {
            PlayerPrefs.SetInt("textToSpeech", 1);
            ttsStatusText.text = "On";
        }
        else
        {
            PlayerPrefs.SetInt("textToSpeech", 0);
            ttsStatusText.text = "Off";
        }
    }

    public void showAccesibility()
    {
        if (PlayerPrefs.GetInt("font_size") == 16)
            fontSizeScrollBar.value = 0;
        else if (PlayerPrefs.GetInt("font_size") == 17)
            fontSizeScrollBar.value = 0.5f;
        else
            fontSizeScrollBar.value = 1;

        if (PlayerPrefs.GetInt("dyslexia") == 1)
            dyslexiaToggle.isOn = true;
        if (PlayerPrefs.GetInt("contrast") == 1)
            contrastToggle.isOn = true;
        if (PlayerPrefs.GetInt("tts") == 1)
            textToSpeechToggle.isOn = true;
    }

    public void hideAccesibility()
    {
        transform.LeanMoveLocal(new Vector2(0, -645), 1).setEaseOutQuart();
        GameObject.Find("AccessHelper").GetComponent<ApplyAccessibility>().ApplyAccessibilitySettings();
    }
}