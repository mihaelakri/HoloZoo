using UnityEngine;
using UnityEngine.UI;

public class HomeLanguage : LanguageBase
{
    [SerializeField]
    Text learn, quiz;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        learn.text = translation.buttons.btn_learn;
        quiz.text = translation.buttons.btn_quiz;
    }
}