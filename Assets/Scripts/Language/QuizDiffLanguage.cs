using UnityEngine;
using UnityEngine.UI;

public class QuizDiffLanguage : LanguageBase
{
    [SerializeField]
    Text choose_difficulty, easy, medium, hard, start_quiz_btn;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        choose_difficulty.text = translation.quiz_scene.choose_difficulty;
        easy.text = translation.quiz_scene.easy;
        medium.text = translation.quiz_scene.medium;
        hard.text = translation.quiz_scene.hard;
        start_quiz_btn.text = translation.buttons.btn_start_quiz;
    }
}