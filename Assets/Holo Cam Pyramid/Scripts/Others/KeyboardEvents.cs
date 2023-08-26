using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Helper struct for configure events 
/// </summary>
[System.Serializable]
public struct EventItem
{
    /// <summary>
    /// Key which Invoke the event
    /// </summary>
    public KeyCode Key;

    /// <summary>
    /// It will Invoke if key in EventItem.Key field is press down
    /// </summary>
    public UnityEvent OnKeyDown;

    /// <summary>
    /// It will Invoke if key in EventItem.Key field is press up.
    /// </summary>
    public UnityEvent OnKeyUp;
}

/// <summary>
/// This is a Helper Script for Invoke events when user Press keyboard keys,
/// it has developed only for debugging purposes.
/// </summary>
public class KeyboardEvents : MonoBehaviour {

    /// <summary>
    /// Array of EventItem struct for handle multiple keyboard events
    /// </summary>
    // Array of EventItem allows configure multiple key events
    public EventItem[] KeyboardEventsItem;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Iterates over all EventItem in keyboardEventsItem array
		foreach (EventItem item in KeyboardEventsItem)
        {
            if (Input.GetKeyDown(item.Key))
            {
                // Invokes the OnKeyDown event in this EventItem
                item.OnKeyDown.Invoke();
            }

            if (Input.GetKeyUp(item.Key))
            {
                // Invokes the OnKeyUp event in this EventItem
                item.OnKeyUp.Invoke();
            }
        }
	}
}
