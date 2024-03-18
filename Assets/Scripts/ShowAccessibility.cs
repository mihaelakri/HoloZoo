using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using CommunicationMsgs;
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
    private Transform accessibilityPopUpTransform;

    void Start()
    {
        sideButtons.gameObject.SetActive(false);
        bottomButtons.gameObject.SetActive(false);

    }

    private void Update()
    {
        GameObject activeParent = GameObject.Find("Learn screen animal- rotate screen");
     
        if (activeParent != null)
        {
   
            accessibilityPopUpTransform = FindInactiveGameObjectByName(activeParent.transform, "AccessibilityPopUp");

            if (accessibilityPopUpTransform != null)
            {
            
                foreach (Transform child in accessibilityPopUpTransform)
                {                  
                    //Debug.Log(child.name);
                }
            }
            else
            {
                Debug.LogError("AccessibilityPopUp not found among the children of the active parent.");
            }
        }
        else
        {
            Debug.LogError("Active parent GameObject not found.");
        }
    }
    Transform FindInactiveGameObjectByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }

            Transform found = FindInactiveGameObjectByName(child, name);
            if (found != null)
            {
                return found;
            }
        }
        return null; // Return null if the GameObject is not found
    }

    public void setStartingAcc(){

        rotationSpeedScrollBar.value = CommConstants.state.initial_rotation_speed-0.5f; 
        
        sideButtons.gameObject.SetActive(false);
        bottomButtons.gameObject.SetActive(false);  
        sideSlider.gameObject.SetActive(true);
        bottomSlider.gameObject.SetActive(true);
       

        Toggle toggleButtonBackground = accessibilityPopUpTransform.Find("Background").transform.Find("Toggle").GetComponent<Toggle>();
       

        toggleButtonBackground.isOn = false;
        // Camera.main.backgroundColor = Color.black;
       
         PlayerPrefs.SetInt("backgorund_white", 0);
      
        Toggle toggleButtonAnimation = accessibilityPopUpTransform.Find("Animation").transform.Find("Toggle").GetComponent<Toggle>();
         toggleButtonAnimation.isOn = false; 

         Toggle toggleButtonBtnSlider = accessibilityPopUpTransform.Find("BtnSlider").transform.GetChild(1).GetComponent<Toggle>();
         toggleButtonBtnSlider.isOn = false;

        CommConstants.rotation.x = 0;
        CommConstants.rotation.y = 0;
        sideSlider.value = 0;
        bottomSlider.value = 0;


    }

    public void SetRotationSpeed(){

        float rotation_speed = rotationSpeedScrollBar.value;
        if(rotation_speed < 0.4f){
            CommConstants.state.initial_rotation_speed = 0.5f;
        }else if(rotation_speed >= 0.4f && rotation_speed < 0.7f){
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
      
        Transform AccessibilityTransform = accessibilityPopUpTransform.Find("Background").GetComponent<Transform>();
        Debug.Log(AccessibilityTransform);
        Image AccessibilityBackground = accessibilityPopUpTransform.GetComponent<Image>();
        

        if (Camera.main.backgroundColor == Color.white)
        {
            Camera.main.backgroundColor = Color.black;
            AccessibilityBackground.color = Color.white;
            PlayerPrefs.SetInt("backgorund_white", 0);
            ChangeTextColorRecursiveInChildren(accessibilityPopUpTransform, Color.black);
            CommConstants.state.background_color = 0;
             foreach (Button button in zoominoutButtons)
            {
                SetButtonColor(button, Color.white);
                
            }
        
        }
        else {
            Camera.main.backgroundColor = Color.white;
            AccessibilityBackground.color = Color.black;
            PlayerPrefs.SetInt("backgorund_white", 1);
            CommConstants.state.background_color = 1;
           
            ChangeTextColorRecursiveInChildren(accessibilityPopUpTransform, Color.white);
             foreach (Button button in zoominoutButtons)
            {
                SetButtonColor(button, Color.black);
                
            }
            
        }

        CommConstants.connection.SendData(CommunicationMsgs.StateMsg);
    }

    public void displayButton()
    {

       Toggle toggleButton = accessibilityPopUpTransform.Find("BtnSlider").transform.GetChild(1).GetComponent<Toggle>();
        
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
        

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"accessibility_view.php", form)){

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
