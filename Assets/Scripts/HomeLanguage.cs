using UnityEngine;
using UnityEngine.UI;

public class HomeLanguage : MonoBehaviour
{
    // home buttons
    public Text learn;
    public Text quiz;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        learn.text = translation.buttons.btn_learn;
        quiz.text = translation.buttons.btn_quiz;
    }
}