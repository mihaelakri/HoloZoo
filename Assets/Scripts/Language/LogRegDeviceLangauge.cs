using UnityEngine;
using UnityEngine.UI;

public class LogRegDeviceLangauge : LanguageBase
{
    [SerializeField]
    Text choose_device, mobile, tablet, login, register;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        choose_device.text = translation.headings.heading_choose_device;
        mobile.text = translation.buttons.btn_game;
        tablet.text = translation.buttons.btn_hologram;
        login.text = translation.buttons.btn_login;
        register.text = translation.buttons.btn_register;
    }
}