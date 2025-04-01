using UnityEngine;
using UnityEngine.UI;

public class QuizScoreLanguage : LanguageBase
{
    [SerializeField]
    Text home_btn, new_quiz_btn;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        home_btn.text = translation.buttons.btn_home;
        new_quiz_btn.text = translation.buttons.btn_new_quiz;
    }
}