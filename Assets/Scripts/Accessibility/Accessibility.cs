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
    public Text contrastText;
    public Text ttsText;
    public Text save_btn;
    public Text small;
    public Text medium;
    public Text large;

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

        save_btn.text = GameData.Instance.translations.buttons.btn_save;
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
        if (PlayerPrefs.GetInt("font_size") == 16)
            fontSizeScrollBar.value = 0;
        else if (PlayerPrefs.GetInt("font_size") == 18)
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
        GameObject.Find("AccessibilityManager").GetComponent<ApplyAccessibility>().ApplyAccessibilitySettings();
    }
}