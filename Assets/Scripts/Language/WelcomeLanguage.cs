using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WelcomeLanguage : MonoBehaviour
{
    public Text welcome;
    public TextMeshProUGUI hello_text;

    public TextMeshProUGUI start_btn;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        hello_text.text = translation.welcome_scene.hello_text;
        start_btn.text = translation.buttons.btn_start;
        welcome.text = translation.welcome_scene.welcome;
    }
}