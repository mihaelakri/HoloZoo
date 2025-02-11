using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalListLanguage : MonoBehaviour
{
      // Accessibility 
    public Text threed_text; 
    public Text endangerment_status; 
    public Text region; 
    public Text habitat; 
    public Text weight; 
    public Text diet; 
    public Text population; 

    // Language
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        { 
            "en", new Dictionary<string, string>
            {
                { "threed_text", "3D View" },
                { "endangerment_status", "Endangerment Status" },
                { "region", "Region" },
                { "habitat", "Habitat" },
                { "weight", "Weight" },
                { "diet", "Diet" },
                { "population", "Population" }
            }
        },
        { "fr", new Dictionary<string, string>
            {
                { "threed_text", "Vue 3D" },
                { "endangerment_status", "Statut de menace" },
                { "region", "Région" },
                { "habitat", "Habitat" },
                { "weight", "Poids" },
                { "diet", "Régime" },
                { "population", "Population" }
            }
        },
        { "hr", new Dictionary<string, string>
            {
                { "threed_text", "3D prikaz" },
                { "endangerment_status", "Status ugroženosti" },
                { "region", "Regija" },
                { "habitat", "Stanište" },
                { "weight", "Težina" },
                { "diet", "Prehrana" },
                { "population", "Populacija" }
            }
        },
        { "es", new Dictionary<string, string>
            {
                { "threed_text", "Vista 3D" },
                { "endangerment_status", "Estado de peligro" },
                { "region", "Región" },
                { "habitat", "Hábitat" },
                { "weight", "Peso" },
                { "diet", "Dieta" },
                { "population", "Población" }
            }
        },
        { "hu", new Dictionary<string, string>
            {
                { "threed_text", "Kattints az állat képére a 3D-s nézet megtekintéséhez" },
                { "endangerment_status", "Veszélyeztetettségi állapot" },
                { "region", "Régió" },
                { "habitat", "Élőhely" },
                { "weight", "Súly" },
                { "diet", "Táplálkozás" },
                { "population", "Populáció" }
            }
        }
    };

    void Start()
    {
        selectedLanguage = PlayerPrefs.GetString("lang", "en");
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        if (translations.ContainsKey(selectedLanguage))
        {
            var languageTexts = translations[selectedLanguage];

            threed_text.text = languageTexts["threed_text"];
            endangerment_status.text = languageTexts["endangerment_status"];
            region.text = languageTexts["region"];
            habitat.text = languageTexts["habitat"];
            weight.text = languageTexts["weight"];
            diet.text = languageTexts["diet"];
            population.text = languageTexts["population"];
 
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
