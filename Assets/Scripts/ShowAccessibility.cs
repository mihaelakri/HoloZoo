using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ShowAccessibility : MonoBehaviour
{
    public GameObject accessibilityScreen;
    private Animator animator;
    public Scrollbar rotationSpeedScrollBar; 
    private bool isAnimationPlaying = false;
    public Slider bottomSlider;
    public Slider sideSlider;
    public GameObject bottomButtons;
    public GameObject sideButtons;
    public Button[] zoominoutButtons; 


    void Start()
    {
        sideButtons.gameObject.SetActive(false);
        bottomButtons.gameObject.SetActive(false);  
    }

    public void setStartingAcc(){

        rotationSpeedScrollBar.value = CommConstants.state.initial_rotation_speed-0.5f; 
        
        sideButtons.gameObject.SetActive(false);
        bottomButtons.gameObject.SetActive(false);  
        sideSlider.gameObject.SetActive(true);
        bottomSlider.gameObject.SetActive(true);

        Toggle toggleButtonBackground = GameObject.Find("Background").transform.GetChild(1).GetComponent<Toggle>();
        toggleButtonBackground.isOn = false; 
        Camera.main.backgroundColor = Color.black;
         PlayerPrefs.SetInt("backgorund_white", 0);

        Toggle toggleButtonAnimation = GameObject.Find("Animation").transform.GetChild(1).GetComponent<Toggle>();
        toggleButtonAnimation.isOn = false; 

        Toggle toggleButtonBtnSlider = GameObject.Find("BtnSlider").transform.GetChild(1).GetComponent<Toggle>();
        toggleButtonBtnSlider.isOn = false; 
    
    }

    public void SetRotationSpeed(){

        float rotation_speed = rotationSpeedScrollBar.value;
        if(rotation_speed < 0.5f){
            CommConstants.state.initial_rotation_speed = 0.5f;
        }else if(rotation_speed >= 0.5f && rotation_speed < 0.8f){
            CommConstants.state.initial_rotation_speed = 1f;
        }else{
             CommConstants.state.initial_rotation_speed = 1.5f;
        }

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
        //Transform AccessibilityTransform = AccessibilityWindow.GetComponent<Transform>();
        //Image AccessibilityBackground = AccessibilityWindow.GetComponent<Image>();
        

        if (Camera.main.backgroundColor == Color.white)
        {
            Camera.main.backgroundColor = Color.black;
           //AccessibilityBackground.color = Color.black;
            PlayerPrefs.SetInt("backgorund_white", 0);
            //ChangeTextColorRecursiveInChildren(AccessibilityTransform, Color.white);
             foreach (Button button in zoominoutButtons)
            {
                SetButtonColor(button, Color.white);
                
            }
        
        }
        else {
            Camera.main.backgroundColor = Color.white;
            //AccessibilityBackground.color = Color.white;
            PlayerPrefs.SetInt("backgorund_white", 1);
            //ChangeTextColorRecursiveInChildren(AccessibilityTransform, Color.black);
             foreach (Button button in zoominoutButtons)
            {
                SetButtonColor(button, Color.black);
                
            }
            
        }
    }

    public void displayButton()
    {

       Toggle toggleButton = GameObject.Find("BtnSlider").transform.GetChild(1).GetComponent<Toggle>();
        
       if (toggleButton.isOn)
            {
                // If toggle is on, show buttons and hide sliders
                sideSlider.gameObject.SetActive(false);
                bottomSlider.gameObject.SetActive(false);
                sideButtons.gameObject.SetActive(true);
                bottomButtons.gameObject.SetActive(true);
                CommConstants.state.control_type = 3;
            }
        else
            {
                // If toggle is off, show sliders and hide buttons
                sideSlider.gameObject.SetActive(true);
                bottomSlider.gameObject.SetActive(true);
                sideButtons.gameObject.SetActive(false);
                bottomButtons.gameObject.SetActive(false);
                CommConstants.state.control_type = 2;
            }
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

    public void saveAccessibilityPrefs(System.Action<int> callback){
        StartCoroutine(screenshotAccessibility(callback));
    }


    IEnumerator screenshotAccessibility(System.Action<int> callback){

        WWWForm form = new WWWForm();
        form.AddField("backgorund_color", PlayerPrefs.GetInt("backgorund_white") == 1 ? "white" : "black");
        form.AddField("rotation_speed", CommConstants.state.initial_rotation_speed.ToString());
        form.AddField("object_size", CommConstants.state.finish_size.ToString());
        form.AddField("animation", isAnimationPlaying ? 1 : 0);
        

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"/accessibility_view.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    int accId = Convert.ToInt16(www.downloadHandler.text);
                    PlayerPrefs.SetInt("accessibilityId", accId);
                    Debug.Log("Accessibility preferences added to database with ID: " + accId);
                    callback(accId);
                }
        }
    }

}
