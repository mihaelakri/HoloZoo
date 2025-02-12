using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoLanguage : MonoBehaviour
{
   //texts
    public Text developed_by; 
    public Text univ_text; 

    // Language
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                { "developed_by", "Developed by" },
                { "univ_text", "University of Dubrovnik, Faculty of electrical engineering and computing as a part of Play2Green project (Serious Gaming for Universal Access to Green Education)." }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "developed_by", "Développé par" },
                { "univ_text", "l'Université de Dubrovnik, Faculté de génie électrique et d'informatique dans le cadre du projet Play2Green (Serious Gaming pour l'accès universel à l'éducation verte)." }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "developed_by", "Aplikaciju razvilo" },
                { "univ_text", "Sveučilište u Dubrovniku, Fakulteta elektrotehnike i računarstva u sklopu projekta Play2Green (Serious Gaming for Universal Access to Green Education)." }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "developed_by", "Desarrollado por" },
                { "univ_text", "la Universidad de Dubrovnik, Facultad de Ingeniería Eléctrica y Computación como parte del proyecto Play2Green (Juegos serios para el acceso universal a la educación verde)." }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "developed_by", "Által kifejlesztett" },
                { "univ_text", "Dubrovniki Egyetem Villamosmérnöki és Számítástechnikai Kar a Play2Green projekt (Serious Gaming for Universal Access to Green Education) részeként." }
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

            developed_by.text = languageTexts["developed_by"];
            univ_text.text = languageTexts["univ_text"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
