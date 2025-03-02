using UnityEngine;
using UnityEngine.UI;

public class LoginLanguage : MonoBehaviour
{
    //Login form
    public Text login_heading;
    public Text username_placeholder;
    public Text password_placeholder;
    public Text register_btn;
    public Text login_btn;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        login_heading.text = translation.headings.heading_login;
        username_placeholder.text = translation.placeholders.placeholder_username;
        password_placeholder.text = translation.placeholders.placeholder_password;
        register_btn.text = translation.buttons.btn_register;
        login_btn.text = translation.buttons.btn_login;
    }
}