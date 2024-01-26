using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAccessibility : MonoBehaviour
{
    public GameObject accessibilityScreen;
    private Animator animator;
    private bool isAnimationPlaying = false;
    private Slider bottomSlider;
    private Slider sideSlider;
    public GameObject bottomButtons;
    public GameObject sideButtons;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {

        sideButtons.gameObject.SetActive(false);
        bottomButtons.gameObject.SetActive(false);

    }

    private void Update()
    {
        
    }
    public void showAccessibilityBtn()
    {
        accessibilityScreen.SetActive(true);
    }

    public void closeAccessibilityMenu()
    {
        accessibilityScreen.SetActive(false);
    }
    public void changeColor()
    {
        GameObject AccessibilityWindow = GameObject.Find("AccessibilityPopUp");
        Transform AccessibilityTransform = AccessibilityWindow.GetComponent<Transform>();
        Image AccessibilityBackground = AccessibilityWindow.GetComponent<Image>();
        

        if (Camera.main.backgroundColor == Color.white)
        {
            Camera.main.backgroundColor = Color.black;
            AccessibilityBackground.color = Color.black;
            ChangeTextColorRecursiveInChildren(AccessibilityTransform, Color.white);

        }
        else {
            Camera.main.backgroundColor = Color.white;
            AccessibilityBackground.color = Color.white;
            ChangeTextColorRecursiveInChildren(AccessibilityTransform, Color.black);
        }
    }

    public void displayButton()
    {

        sideSlider = GameObject.Find("SideSlider").GetComponent<Slider>();
        bottomSlider = GameObject.Find("BottomSlider").GetComponent<Slider>();
        bottomSlider.gameObject.SetActive(false);
        sideSlider.gameObject.SetActive(false);

        sideButtons.gameObject.SetActive(true);
        bottomButtons.gameObject.SetActive(true);

    }

    public void animationHandler() {
        Transform modelTransform = GameObject.Find("Model").transform.GetChild(0);
        animator = modelTransform.GetComponent<Animator>();

        // Check if an Animator component is found
        if (animator != null)
        {
            if (isAnimationPlaying)
            {
                animator.enabled = true;
                Debug.Log("Animation is playing");
                isAnimationPlaying = false;

            }
            else
            {
                // Animation is not playing
                animator.enabled = false; ;
                Debug.Log("Animation is not playing");
                isAnimationPlaying = true;
            }
        }
        else
        {
            Debug.LogWarning("Animator component not found on this GameObject.");
        }

 
    }

    private void ChangeTextColorRecursiveInChildren(Transform parent, Color newColor)
    {
        // Change text color for the current object if it has a Text component
        Text textComponent = parent.GetComponent<Text>();
        if (textComponent != null)
        {
            textComponent.color = newColor;
        }

        // Recursively call the function for each child
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            ChangeTextColorRecursiveInChildren(child, newColor);
        }
    }




}
