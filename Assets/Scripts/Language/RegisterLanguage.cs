using UnityEngine;
using UnityEngine.UI;

public class RegisterLanguage : LanguageBase
{
    [SerializeField]
    Text register_heading, username_placeholder, password_placeholder, password_repeat_placeholder, register_btn, privacy_policy_text;

    protected override void ApplyLanguageTexts()
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