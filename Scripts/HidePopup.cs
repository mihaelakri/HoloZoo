using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePopup : MonoBehaviour
{

    public GameObject prefabPanel; // Reference to the instantiated prefab

    // Method to hide the instantiated prefab
    public void HidePrefab()
    {
        prefabPanel.SetActive(false); // Set the prefab's active state to false
    }
}
