using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloAccessibility : MonoBehaviour
{
    private GameObject HoloCamera;

    // Start is called before the first frame update
    void Start()
    {
        HoloCamera = GameObject.Find("HologramCamera");
        CommConstants.state.OnUpdated += HoloAccess_OnStateUpdated;

    }

    public void HoloAccess_OnStateUpdated()
    {
        changeBg(CommConstants.state.background_color);
        System.Diagnostics.Debug.WriteLine("HoloAccess. StateUpdated: " + CommConstants.state.background_color);
    }

    private void changeBg(int color)
    {
        
        foreach (Transform child in HoloCamera.transform)
        {       
           if (color == 1)
            {
                child.gameObject.GetComponent<Camera>().backgroundColor = Color.white;
                Camera.main.backgroundColor = Color.white;
            }
            else
            {
                child.gameObject.GetComponent<Camera>().backgroundColor = Color.black;
                Camera.main.backgroundColor = Color.black;
            }
        }
    }
}
