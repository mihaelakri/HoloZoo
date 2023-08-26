using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets the background of the visualizer to black when enter to play mode.
/// </summary>
public class SetVisualizerBGToBlack : MonoBehaviour {

    private Image img;

	// Use this for initialization
	void Start () {
        img = gameObject.GetComponent<Image>();
        img.color = Color.black;
        img.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
