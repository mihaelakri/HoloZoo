using UnityEngine;
using UnityEngine.UI;

public class RegisterLanguage : MonoBehaviour
{
    // Register Form
    public Text register_heading;
    public Text username_placeholder;
    public Text password_placeholder;
    public Text password_repeat_placeholder;
    public Text register_btn;
    public Text privacy_policy_text;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        register_heading.text = translation.headings.heading_register;
        username_placeholder.text = translation.placeholders.placeholder_username;
        password_placeholder.text = translation.placeholders.placeholder_password;
        password_repeat_placeholder.text = translation.placeholders.placeholder_password_repeat;
        register_btn.text = translation.buttons.btn_register;
        privacy_policy_text.text = translation.registration_scene.privacy_policy_text;

    }
}