using UnityEngine;
using UnityEngine.UI;

public class InfoLanguage : LanguageBase
{
    [SerializeField]
    Text developed_by, univ_text;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        developed_by.text = translation.info_scene.developed_by;
        univ_text.text = translation.info_scene.univ_text;
    }
}