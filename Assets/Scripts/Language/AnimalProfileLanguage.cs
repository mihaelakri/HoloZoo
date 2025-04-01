using UnityEngine;
using UnityEngine.UI;

public class AnimalProfileLanguage : LanguageBase
{
    [SerializeField]
    Text threed_text, general_info, endangerment_status, region, habitat, weight, diet, population;

    protected override void ApplyLanguageTexts()
    {
        var translation = GameData.Instance.translations;

        threed_text.text = translation.animal_profile_scene.threed_text;
        general_info.text = translation.animal_profile_scene.general_info;
        endangerment_status.text = translation.animal_profile_scene.endangerment_status;
        region.text = translation.animal_profile_scene.region;
        habitat.text = translation.animal_profile_scene.habitat;
        weight.text = translation.animal_profile_scene.weight;
        diet.text = translation.animal_profile_scene.diet;
        population.text = translation.animal_profile_scene.population;
    }
}