using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class ApplyAccessibility : MonoBehaviour
{
    public static ApplyAccessibility Instance; // Singleton pristup

    public Font openDyslexic;
    public Font jostFont;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Održava postavke između scena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyAccessibilitySettings(); // Primijeni postavke kad se scena učita
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        ApplyAccessibilitySettings(); // Primijeni postavke na početku
    }

    public void ApplyAccessibilitySettings()
    {
        // Text[] allTexts = FindObjectsOfType<Text>();
        // Button[] allButtons = FindObjectsOfType<Button>();
        Text[] textsDark = GameObject.FindGameObjectsWithTag("text-dark").Select(o => o.GetComponent<Text>()).ToArray();
        Text[] textsLight = GameObject.FindGameObjectsWithTag("text-light").Select(o => o.GetComponent<Text>()).ToArray();

        Image[] buttonsDark = GameObject.FindGameObjectsWithTag("img-dark").Select(o =>
        {
            if (!o.TryGetComponent<Image>(out var img))
            {
                // Some buttons have Image component as a child
                img = o.GetComponentInChildren<Image>();
            }

            if (img == null)
            {
                Debug.Log("Scene: [" + SceneManager.GetActiveScene().name + "] Object: [" + o.name + "] doesn't have an image component");
            }

            return img;
        }).ToArray();

        Image[] buttonsLight = GameObject.FindGameObjectsWithTag("img-light").Select(o =>
        {
            if (!o.TryGetComponent<Image>(out var img))
            {
                // Some buttons have Image component as a child
                img = o.GetComponentInChildren<Image>();
            }

            if (img == null)
            {
                Debug.Log("Scene: [" + SceneManager.GetActiveScene().name + "] Object: [" + o.name + "] doesn't have an image component");
            }

            return img;
        }
        ).ToArray();

        int fontSize = PlayerPrefs.GetInt("font_size", 14); // Default 14 ako nije postavljeno
        int dyslexiaEnabled = PlayerPrefs.GetInt("dyslexia", 0);
        int contrastEnabled = PlayerPrefs.GetInt("contrast", 0);

        Font selectedFont = (dyslexiaEnabled == 1) ? openDyslexic : jostFont;

        // Postavi veličinu fonta i zamijeni font
        foreach (Text text in textsDark.Concat(textsLight))
        {
            text.fontSize = fontSize;
            text.font = selectedFont;
        }

        // Postavi kontrast (crna boja na gumbovima i tekstovima osim "CONFIRM")
        if (contrastEnabled == 1)
        {
            foreach (Text text in textsDark)
            {
                text.color = Color.black;
            }

            foreach (Text text in textsLight)
            {
                text.color = Color.white;
            }

            foreach (Image img in buttonsDark)
            {
                img.color = Color.black;
            }

            foreach (Image img in buttonsLight)
            {
                img.color = Color.white;
            }
        }
    }
}