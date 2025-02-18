using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        Text[] allTexts = FindObjectsOfType<Text>();
        Button[] allButtons = FindObjectsOfType<Button>();

        int fontSize = PlayerPrefs.GetInt("font_size", 14); // Default 14 ako nije postavljeno
        int dyslexiaEnabled = PlayerPrefs.GetInt("dyslexia", 0);
        int contrastEnabled = PlayerPrefs.GetInt("contrast", 0);

        Font selectedFont = (dyslexiaEnabled == 1) ? openDyslexic : jostFont;

        // Postavi veličinu fonta i zamijeni font
        foreach (Text text in allTexts)
        {
            text.fontSize = fontSize;
            text.font = selectedFont;
        }

        // Postavi kontrast (crna boja na gumbovima i tekstovima osim "CONFIRM")
        if (contrastEnabled == 1)
        {
            foreach (Button btn in allButtons)
            {
                btn.GetComponent<Image>().color = Color.black;
            }

            foreach (Text text in allTexts)
            {
                text.color = Color.black;

            }
        }
    }
}