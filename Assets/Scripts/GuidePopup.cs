using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GuidePopup : MonoBehaviour
{
    public Text infoText;
    public Button nextButton;
    public Button backButton; // Reference to the back button
    public Button accessibilityButton; // Reference to the accessibility button
    public Button repeatButton; // Reference to the repeat button
    public Button[] rotationButtons; // References to the rotation method buttons
    public Button[] zoominoutButtons; 
    public Slider bottomSlider;
    public Slider sideSlider;
    public HologramQuizIntro hologramQuizIntro; 

    public GameObject accessibilityPopup; 

    private List<string> infoList;
    private int currentIndex = 0;

    private Color normalColor = Color.white;
    private Color highlightColor = Color.yellow;

    void Start()
    {
        // Initialize your list of info texts
        infoList = new List<string>();
        infoList.Add("Pozdrav, ovo je kratki vodič za prikaz glavnih komponenti kviza.");
        infoList.Add("U gornjem lijevom kutu zaslona nalazi se gumb NAZAD za napuštanje kviza."); //1
        infoList.Add("U gornjem srednjem dijelu zaslona nalazi se gumb PRISTUPNOST za promjenu opcija pristupačnosti."); //2
        infoList.Add("U gornjem desnom dijelu zaslona nalazi se gumb PONOVNO POSTAVI PITANJE."); //3
        infoList.Add("Možete birati između tri različite metode za rotiranje 3D objekata:");
        infoList.Add("1. leap motion\n2. Klizači\n3. Gumbi");
        infoList.Add("Gumbi se mogu aktivirati u postavkama pristupačnosti."); //6
        infoList.Add("Idemo isprobati opcije pristupačnosti."); // 7
        infoList.Add("Možete promijeniti veličinu životinje pomoću gumba Povećaj i Smanji."); //8
        infoList.Add("Idemo isprobati demo verziju.");
        // Add more info texts as needed

        // Set the initial text
        infoText.text = infoList[currentIndex];

        // Hook up the button click event
        nextButton.onClick.AddListener(NextInfo);
    }

    void NextInfo()
    {
        // Increment index and check if it's out of range
        currentIndex++;
        if (currentIndex >= infoList.Count)
        {
            hologramQuizIntro.stopTimerIntro();
            hologramQuizIntro.startDemo(); 
            gameObject.SetActive(false);
            
        }
        else
        {
            // Update the text
            infoText.text = infoList[currentIndex];
            UpdateButtonAppearance();
        }
    }

    void UpdateButtonAppearance()
        {
            // Reset all buttons to normal state
            SetButtonColor(backButton, normalColor);
            SetButtonColor(accessibilityButton, normalColor);
            SetButtonColor(repeatButton, normalColor);
            foreach (Button button in rotationButtons)
            {
                 button.transform.parent.gameObject.SetActive(false);
            }
            foreach (Button button in zoominoutButtons)
            {
                SetButtonColor(button, normalColor);
                
            }
             sideSlider.gameObject.SetActive(true); 
             bottomSlider.gameObject.SetActive(true);

            // Highlight buttons based on current step
            switch (currentIndex)
            {
                case 1: // Step 1
                    SetButtonColor(backButton, highlightColor);
                    break;
                case 2: // Step 2
                    SetButtonColor(accessibilityButton, highlightColor);
                    break;
                case 3: // Step 3
                    SetButtonColor(repeatButton, highlightColor);
                    break;
                case 6: // Step 6
                    foreach (Button button in rotationButtons)
                    {
                        SetButtonColor(accessibilityButton, highlightColor);
                        sideSlider.gameObject.SetActive(false); 
                        bottomSlider.gameObject.SetActive(false);
                        button.transform.parent.gameObject.SetActive(true);
                    }
                    break;
                case 8: // Step 6
                    foreach (Button button in zoominoutButtons)
                    {
                        SetButtonColor(button, highlightColor);
                    }
                    accessibilityPopup.SetActive(true);
                    break;
                    
            }
        }

        void SetButtonColor(Button button, Color color)
        {
            // Get the image component of the button
            Image buttonImage = button.GetComponent<Image>();

            // Set the color of the button's image
            if (buttonImage != null)
            {
                buttonImage.color = color;
            }
            else
            {
                Debug.LogError("Button does not have an Image component.");
            }
        }
}
