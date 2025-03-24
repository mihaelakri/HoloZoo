using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Accessibility : MonoBehaviour
{
    [SerializeField]
    Scrollbar fontSizeScrollBar;
    [SerializeField]
    Toggle dyslexiaToggle, contrastToggle, textToSpeechToggle;
    [SerializeField]
    Text font_size, dyslexiaText, contrastText, ttsText, save_btn, small, medium, large;
    [SerializeField]
    UnityEngine.UI.Dropdown dropdown;
    readonly List<string> languages = new() { "en", "fr", "hr", "es", "hu" };
    bool languageChanged = false;

    void Start()
    {
        ApplyLanguageTexts();
    }

    private void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        font_size.text = translation.accessibility.font_size;
        dyslexiaText.text = translation.accessibility.dyslexia;
        contrastText.text = translation.accessibility.contrast_text;
        ttsText.text = translation.accessibility.tts_text;
        small.text = translation.accessibility.small;
        medium.text = translation.accessibility.medium;
        large.text = translation.accessibility.large;

        save_btn.text = translation.buttons.btn_save;
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
            PlayerPrefs.SetInt("dyslexia", 1);
        }
        else
        {
            PlayerPrefs.SetInt("dyslexia", 0);
        }
    }

    public void SetContrast()
    {
        if (contrastToggle.isOn)
        {
            PlayerPrefs.SetInt("contrast", 1);
        }
        else
        {
            PlayerPrefs.SetInt("contrast", 0);
        }
    }

    public void SetTextToSpeech()
    {
        if (textToSpeechToggle.isOn)
        {
            PlayerPrefs.SetInt("textToSpeech", 1);
        }
        else
        {
            PlayerPrefs.SetInt("textToSpeech", 0);
        }
    }

    public void showAccesibility()
    {
        int fontSize = PlayerPrefs.GetInt("font_size", 18);

        if (fontSize == 16)
            fontSizeScrollBar.value = 0;
        else if (fontSize == 18)
            fontSizeScrollBar.value = 0.5f;
        else
            fontSizeScrollBar.value = 1;

        if (PlayerPrefs.GetInt("dyslexia") == 1)
            dyslexiaToggle.isOn = true;
        if (PlayerPrefs.GetInt("contrast") == 1)
            contrastToggle.isOn = true;
        if (PlayerPrefs.GetInt("tts") == 1)
            textToSpeechToggle.isOn = true;

        dropdown.value = languages.IndexOf(PlayerPrefs.GetString("lang", "en"));
    }

    public void hideAccesibility()
    {
        transform.LeanMoveLocal(new Vector2(0, -645), 1).setEaseOutQuart();
        ApplyAccessibility.Instance.ApplyAccessibilitySettings();

        if (languageChanged)
        {
            ApplyAccessibility.Instance.OnLanguageChanged();
            languageChanged = false;
        }
    }

    public void SetLanguageByIndex(int langIndex)
    {
        languageChanged = true;
        string languageCode = languages[langIndex];

        // Spremanje jezika lokalno u PlayerPrefs
        PlayerPrefs.SetString("lang", languageCode);
        PlayerPrefs.Save();
        StartCoroutine(GameData.Instance.LoadTranslatedTables(languageCode));
        Debug.Log($"{nameof(Accessibility)} - Language '{languageCode}' successfully set");
    }
}