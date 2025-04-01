using UnityEngine;
using UnityEngine.UI;

public class AccessibilityPanelLanguage : LanguageBase
{
    [SerializeField]
    Text font_size, dyslexiaText, contrastText, ttsText, save_btn, small, medium, large;

    protected override void ApplyLanguageTexts()
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
}