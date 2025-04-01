using UnityEngine;
using UnityEngine.UI;

public class ProfileLanguage : LanguageBase
{
    [SerializeField]
    Text level, experience, new_password, reapeat_password, confirm;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        level.text = translation.profile_scene.level;
        experience.text = translation.profile_scene.experience;
        new_password.text = translation.placeholders.placeholder_new_password;
        reapeat_password.text = translation.placeholders.placeholder_repeat_new_password;
        confirm.text = translation.buttons.btn_confirm;
    }
}