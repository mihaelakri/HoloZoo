using UnityEngine;
using UnityEngine.UI;

public class LearningModeLanguage : MonoBehaviour
{
    public Text intro_cm;

    // Mode buttons
    public Text globus_mode;
    public Text list_mode;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        intro_cm.text = translation.headings.heading_learn_method;
        globus_mode.text = translation.buttons.btn_globus_mode;
        list_mode.text = translation.buttons.btn_list_mode;
    }
}