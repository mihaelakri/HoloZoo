using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ControlTracker : MonoBehaviour
{
    // Tracking info variables 
    public bool leap_motion; // true is used / false is not used
    public float leap_motion_time; 
    public bool scroll_bar; // true is used / false is not used
    public float scroll_bar_time; 
    public bool buttons; // true is used / false is not used
    public float buttons_time;

    // Slider GameObjects
    public Slider bottomSlider;
    public Slider sideSlider;

     // Unity UI button GameObjects
    public GameObject sideButtons;
    public GameObject bottomButtons;

    private Button[] sideButtonComponents;
    private Button[] bottomButtonComponents;


    void Start()
    {
        // Initialize control usage times to 0
        leap_motion_time = 0f;
        scroll_bar_time = 0f;
        buttons_time = 0f;

        // Get Button components of UI buttons
        sideButtonComponents = sideButtons.GetComponentsInChildren<Button>();
        bottomButtonComponents = bottomButtons.GetComponentsInChildren<Button>();

        // Subscribe to onClick events of UI buttons
        foreach (Button button in sideButtonComponents)
        {
            button.onClick.AddListener(() => OnButtonClicked(button));
        }
        foreach (Button button in bottomButtonComponents)
        {
            button.onClick.AddListener(() => OnButtonClicked(button));
        }
    }


    void Update()
    {
        if(CommConstants.state.control_type == 3){
            buttons = true;
            buttons_time += Time.deltaTime; 
        }
        if(CommConstants.state.control_type == 1){
            leap_motion = true;
            leap_motion_time += Time.deltaTime;
        }else{
            if (bottomSlider != null && bottomSlider.value != bottomSlider.minValue)
                {
                    scroll_bar = true;
                    scroll_bar_time += Time.deltaTime;
                }
                else
                {
                    scroll_bar = false;
                }

                // Check if the side slider is being used
                if (sideSlider != null && sideSlider.value != sideSlider.minValue)
                {
                    scroll_bar = true;
                    scroll_bar_time += Time.deltaTime;
                }
                else
                {
                    scroll_bar = false;
                }
        }
    }

     void OnButtonClicked(Button button)
    {
        buttons = true;
        CommConstants.state.control_type = 3;
    }

    // Method to save control usage data to server
    public void SaveControls(System.Action<int> callback)
    {
        StartCoroutine(SaveControlsPrefs(callback)); 
    }

    IEnumerator SaveControlsPrefs(System.Action<int> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("leap_motion", leap_motion ? 1 : 0);
        form.AddField("leap_motion_time", (leap_motion_time*1000).ToString()); // convert to miliseconds
        form.AddField("scroll_bar", scroll_bar ? 1 : 0);
        form.AddField("scroll_bar_time", (scroll_bar_time*1000).ToString()); // convert to miliseconds
        form.AddField("buttons", buttons ? 1 : 0);
        form.AddField("buttons_time", (buttons_time*1000).ToString()); // convert to miliseconds
        
        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "/controls_view.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // resetiraj vrijednosti
                leap_motion_time = 0f;
                scroll_bar_time = 0f;
                buttons_time = 0f;

                // provjera je li uneseno 
                int controlsId = Convert.ToInt16(www.downloadHandler.text);
                PlayerPrefs.SetInt("controlId", controlsId);
                Debug.Log("Controls added to database with ID: " + controlsId);
                callback(controlsId);
            }
        }
    }
}
