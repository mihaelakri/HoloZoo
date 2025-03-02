using UnityEngine;
using UnityEngine.UI;

public class InfoLanguage : MonoBehaviour
{
    //texts
    public Text developed_by;
    public Text univ_text;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        developed_by.text = translation.info_scene.developed_by;
        univ_text.text = translation.info_scene.univ_text;
    }
}