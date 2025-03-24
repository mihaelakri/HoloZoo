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
    UnityEngine.UI.Dropdown dropdown;
    readonly List<string> languages = new() { "en", "fr", "hr", "es", "hu" };
    bool languageChanged = false;

    public void showAccesibility()
    {
        int fontSize = PlayerPrefs.GetInt("font_size", 18);

        if (fontSize == 16)
            fontSizeScrollBar.value = 0;
        else if (fontSize == 18)
            fontSizeScrollBar.value = 0.5f;
        else
            fontSizeScrollBar.value = 1;

        dyslexiaToggle.isOn = PlayerPrefs.GetInt("dyslexia") == 1;
        contrastToggle.isOn = PlayerPrefs.GetInt("contrast") == 1;
        textToSpeechToggle.isOn = PlayerPrefs.GetInt("tts") == 1;

        dropdown.value = languages.IndexOf(PlayerPrefs.GetString("lang", "en"));
    }

    private void SaveSettings()
    {
        float fontSize = fontSizeScrollBar.value;
        if (fontSize < 0.5f)
            PlayerPrefs.SetInt("font_size", 16);
        else if (fontSize >= 0.5f && fontSize < 0.8)
            PlayerPrefs.SetInt("font_size", 18);
        else
            PlayerPrefs.SetInt("font_size", 20);

        PlayerPrefs.SetInt("dyslexia", dyslexiaToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("contrast", contrastToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("textToSpeech", textToSpeechToggle.isOn ? 1 : 0);
    }

    public void hideAccesibility()
    {
        SaveSettings();
        transform.LeanMoveLocal(new Vector2(0, -645), 1).setEaseOutQuart();
        ApplyAccessibility.Instance.LoadTexts();
        ApplyAccessibility.Instance.ApplyAccessibilitySettings();

        if (languageChanged)
        {
            ApplyAccessibility.Instance.OnLanguageChanged();
            languageChanged = false;
        }
    }

    // Called when different language selected
    public void SetLanguageByIndex(int langIndex)
    {
        string languageCode = languages[langIndex];
        languageChanged = PlayerPrefs.GetString("lang", "en") != languageCode;

        // Spremanje jezika lokalno u PlayerPrefs
        PlayerPrefs.SetString("lang", languageCode);
        PlayerPrefs.Save();
        StartCoroutine(GameData.Instance.LoadTranslatedTables(languageCode));
        Debug.Log($"{nameof(Accessibility)} - Language '{languageCode}' successfully set");
    }

    // Called when dropdown is opened
    public void LanguageDropdownOnClick()
    {
        ApplyAccessibility.Instance.LoadTexts();
        ApplyAccessibility.Instance.ApplyAccessibilitySettings();
        Debug.Log($"{nameof(Accessibility)} - {nameof(LanguageDropdownOnClick)}");
    }
}