using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WelcomeLanguage : MonoBehaviour
{
    public Text welcome; 
    public TextMeshProUGUI hello_text; 

     public TextMeshProUGUI start_btn; 
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                { "hello_text", "Hello! HoloZoo is an educational game about animals and their endangerment status in nature.\n\nThe game uses two Android mobile devices that need to be connected via Bluetooth to display a hologram you can interact with.\n\nDon't worry! If you don't want to use two devices, the hologram will still be displayed on this device.\n\nLearn how to create a pyramidal hologram in the following steps." },
                { "start_btn", "START" },
                { "welcome", "WELCOME" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "hello_text", "Bonjour ! HoloZoo est un jeu éducatif sur les animaux et leur statut de mise en danger dans la nature.\n\nLe jeu utilise deux appareils Android qui doivent être connectés via Bluetooth pour afficher un hologramme avec lequel vous pouvez interagir.\n\nNe vous inquiétez pas ! Si vous ne souhaitez pas utiliser deux appareils, l'hologramme sera toujours affiché sur cet appareil.\n\nApprenez à créer un hologramme pyramidal dans les étapes suivantes." },
                { "start_btn", "DÉMARRER" },
                { "welcome", "BIENVENUE" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "hello_text", "Pozdrav! HoloZoo je edukativna igra o životinjama i njihovoj ugroženosti u prirodi.\n\nIgra koristi dva Android mobilna uređaja koja moraju biti povezana putem Bluetooth-a kako bi prikazala hologram s kojim se možete igrati.\n\nNe brinite! Ako ne želite koristiti dva uređaja, hologram će biti prikazan na ovom uređaju.\n\nNaučite kako stvoriti piramidalni hologram u sljedećim koracima." },
                { "start_btn", "POČNI" },
                { "welcome", "DOBRODOŠLI" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "hello_text", "¡Hola! HoloZoo es un juego educativo sobre los animales y su estado de amenaza en la naturaleza.\n\nEl juego utiliza dos dispositivos Android que deben estar conectados a través de Bluetooth para mostrar un holograma con el que puedes interactuar.\n\nNo te preocupes. Si no deseas usar dos dispositivos, el holograma se mostrará en este dispositivo.\n\nAprende a crear un holograma piramidal en los siguientes pasos." },
                { "start_btn", "COMENZAR" },
                { "welcome", "BIENVENIDO" }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "hello_text", "Helló! A HoloZoo egy oktató játék az állatokról és azok természetben való veszélyeztetettségi státuszáról.\n\nA játék két Android mobil eszközt használ, amelyek Bluetooth-on keresztül kell kapcsolódjanak, hogy megjeleníthessenek egy hologramot, amellyel interakcióba léphetsz.\n\nNe aggódj! Ha nem szeretnél két eszközt használni, a hologram továbbra is megjelenik ezen az eszközön.\n\nTudd meg, hogyan készíthetsz piramis alakú hologramot a következő lépésekben." },
                { "start_btn", "KEZDÉS" },
                { "welcome", "ÜDVÖZÖLJÜK" }
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

            hello_text.text = languageTexts["hello_text"];
            start_btn.text = languageTexts["start_btn"];
            welcome.text = languageTexts["welcome"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}
