using UnityEngine;
using UnityEngine.UI;

public class LoginLanguage : LanguageBase
{
    [SerializeField]
    Text login_heading, username_placeholder, password_placeholder, register_btn, login_btn;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        login_heading.text = translation.headings.heading_login;
        username_placeholder.text = translation.placeholders.placeholder_username;
        password_placeholder.text = translation.placeholders.placeholder_password;
        register_btn.text = translation.buttons.btn_register;
        login_btn.text = translation.buttons.btn_login;
    }
}