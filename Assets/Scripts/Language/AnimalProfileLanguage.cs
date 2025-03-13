using UnityEngine;
using UnityEngine.UI;

public class AnimalProfileLanguage : MonoBehaviour
{
    // Accessibility 
    public Text threed_text;
    public Text endangerment_status;
    public Text region;
    public Text habitat;
    public Text weight;
    public Text diet;
    public Text population;

    void Start()
    {
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        threed_text.text = translation.animal_profile_scene.threed_text;
        endangerment_status.text = translation.animal_profile_scene.endangerment_status;
        region.text = translation.animal_profile_scene.region;
        habitat.text = translation.animal_profile_scene.habitat;
        weight.text = translation.animal_profile_scene.weight;
        diet.text = translation.animal_profile_scene.diet;
        population.text = translation.animal_profile_scene.population;
    }
}