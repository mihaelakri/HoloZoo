using UnityEngine;

/// <summary>
/// Hides the Pyramid Guide Lines after the time it has specified in 
/// VisibleTime field from the start of scene has transcurred.
/// </summary>
public class HidePyramidGuideLines : MonoBehaviour {

    /// <summary>
    /// Time during which Pyramid Guide Lines will be visible
    /// </summary>
    [Tooltip("Line guides will be visible on playing mode during this time (In Seconds)")]
    public float VisibleTime = 5.0f; // Time is in seconds
    
    // TODO: Evaluate a reimplementation of this script using Multi Threading
    private float StartTime;
    private float DeadTime;

	// Use this for initialization
	void Start () {
        // Saves the time when this GameObject is instantiate
        StartTime = Time.time;
        // Calculates the time when GameObject will get disabled
        DeadTime = StartTime + VisibleTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > DeadTime)
        {
            // Hides this game object
            gameObject.SetActive(false);
        }
	}
}
