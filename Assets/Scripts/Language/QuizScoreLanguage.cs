using UnityEngine;
using UnityEngine.UI;

public class QuizScoreLanguage : MonoBehaviour
{
    // Quiz Score Buttons
    public Text home_btn;
    public Text new_quiz_btn;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        home_btn.text = translation.buttons.btn_home;
        new_quiz_btn.text = translation.buttons.btn_new_quiz;
    }
}