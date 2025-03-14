using UnityEngine;
using UnityEngine.UI;

public class AnimalListLanguage : MonoBehaviour
{
   // Accessibility 
    public Text list_text;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        list_text.text = translation.animal_list_scene.list_text;
       
    }
}
