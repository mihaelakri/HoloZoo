using UnityEngine;
using UnityEngine.UI;

public class LogRegDeviceLangauge : MonoBehaviour
{
    //Pop up window (Choose device) 
    public Text choose_device;
    public Text mobile;
    public Text tablet;

    // Buttons (log in/ register)
    public Text login;
    public Text register;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        choose_device.text = translation.headings.heading_choose_device;
        mobile.text = translation.buttons.btn_game;
        tablet.text = translation.buttons.btn_hologram;
        login.text = translation.buttons.btn_login;
        register.text = translation.buttons.btn_login;
    }
}