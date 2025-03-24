using UnityEngine;
using UnityEngine.UI;

public class LearningModeLanguage : LanguageBase
{
    [SerializeField]
    Text intro_cm, globus_mode, list_mode;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        intro_cm.text = translation.headings.heading_learn_method;
        globus_mode.text = translation.buttons.btn_globus_mode;
        list_mode.text = translation.buttons.btn_list_mode;
    }
}