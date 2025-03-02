using UnityEngine;
using UnityEngine.UI;

public class ProfileLanguage : MonoBehaviour
{
    //txts
    public Text level;
    public Text experience;
    public Text new_password;
    public Text reapeat_password;

    // Button
    public Text confirm;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        level.text = translation.profile_scene.level;
        experience.text = translation.profile_scene.experience;
        new_password.text = translation.placeholders.placeholder_new_password;
        reapeat_password.text = translation.placeholders.placeholder_repeat_new_password;
        confirm.text = translation.buttons.btn_confirm;
    }
}