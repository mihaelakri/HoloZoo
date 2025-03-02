using UnityEngine;
using UnityEngine.UI;

public class QuizDiffLanguage : MonoBehaviour
{
    // Choose diffciulty 
    public Text choose_difficulty;
    public Text easy;
    public Text medium;
    public Text hard;
    public Text start_quiz_btn;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        choose_difficulty.text = translation.quiz_scene.choose_difficulty;
        easy.text = translation.quiz_scene.easy;
        medium.text = translation.quiz_scene.medium;
        hard.text = translation.quiz_scene.hard;
        start_quiz_btn.text = translation.buttons.btn_start_quiz;
    }
}