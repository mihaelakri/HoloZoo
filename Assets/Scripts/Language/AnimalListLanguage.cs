using UnityEngine;
using UnityEngine.UI;

public class AnimalListLanguage : LanguageBase
{
    [SerializeField]
    Text list_text;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        list_text.text = translation.animal_list_scene.list_text;
    }
}