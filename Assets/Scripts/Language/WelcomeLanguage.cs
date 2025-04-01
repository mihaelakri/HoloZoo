using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WelcomeLanguage : LanguageBase
{
    [SerializeField]
    Text welcome;
    [SerializeField]
    TextMeshProUGUI hello_text, start_btn;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        hello_text.text = translation.welcome_scene.hello_text;
        start_btn.text = translation.buttons.btn_start;
        welcome.text = translation.welcome_scene.welcome;
    }
}