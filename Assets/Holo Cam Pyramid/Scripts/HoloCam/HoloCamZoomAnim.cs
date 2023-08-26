using UnityEngine;

// TODO: See about documentation printing from VisualStudio https://goo.gl/AyPQ3K


/// <summary>
/// Enum for control the animation status
/// </summary>
public enum AnimationStatus
{
    /// <summary>
    /// Animation is stopped at begin
    /// </summary>
    AnimationBegin,
    /// <summary>
    /// Animation is playing from end to begin.
    /// </summary>
    PlayingAnimationInRevere,
    /// <summary>
    /// Animation is playing from begin to end
    /// </summary>
    PlayingAnimation,
    /// <summary>
    /// Animation is stopped at end
    /// </summary>
    AnimationEnd
}

/// <summary>
/// Helper Script for animate the pyramidal camera
/// </summary>
/// <remarks>
/// This is not the only way of animate the camera zoom,
/// users can implement his own way of achieve the same.
/// 
/// We would like to listen your ideas about this topic, please send us a email to: gamebite10@gmail.com
/// </remarks>
// HoloCamZoomAnim requires the GameObject to have an HoloCameraController
[RequireComponent(typeof(HoloCameraController))]
public class HoloCamZoomAnim : MonoBehaviour {
    /// <summary>
    /// Initial Zoom value for the animation
    /// </summary>
    public float InitZoom = 1.0f;
    /// <summary>
    /// Final Zoom value for the animation
    /// </summary>
    public float FinalZoom = 2.0f;
    /// <summary>
    /// Animation duration from start to end (If PingPong field is enabled animation duration will be duplicated)
    /// </summary>
    [Tooltip("Animation duration in seconds")]
    public float Duration = 3.0f;
    /// <summary>
    /// Execute the animation in reverse when reaches the End status: (begin->end->begin)
    /// </summary>
    [Tooltip("Execute animation normally and on his end execute it in reverse. If PingPong is enabled then animation duration will be: Duration * 2")]
    public bool PingPong = false;

    private AnimationStatus ActualAnimStatus = AnimationStatus.AnimationBegin;
    private float StartTime;
    private HoloCameraController CamCtrl;

	// Use this for initialization
	void Start () {
        // Saves time at initialization
        StartTime = Time.time;
        // Gets a reference of the HoloCameraController for access his SetCamToCenterDistance( float ) method
        CamCtrl = gameObject.GetComponent<HoloCameraController>();
	}
	
	// Update is called once per frame
	void Update () {
        // The animation is controlled by a basic state machine
        switch (ActualAnimStatus)
        {
            case AnimationStatus.AnimationBegin:
                SetInitValues();
                break;
            case AnimationStatus.PlayingAnimation:
                RunAnimation();
                break;
            case AnimationStatus.PlayingAnimationInRevere:
                RunReverseAnimation();
                break;
            case AnimationStatus.AnimationEnd:
                break;
            default:
                break;
        }
	}

    /// <summary>
    /// Starts animation
    /// NOTE: Animation can start only when AnimationStatus is in start or end status.
    /// </summary>
    [ContextMenu("Play Animation")]
    public void PlayAnim()
    {
        if (ActualAnimStatus == AnimationStatus.AnimationBegin || ActualAnimStatus == AnimationStatus.AnimationBegin)
        {
            ActualAnimStatus = AnimationStatus.PlayingAnimation;
            StartTime = Time.time;
        }

    }

    /// <summary>
    /// Stops and set animation status to the begining
    /// NOTE: User can stop animation in any moment.
    /// </summary>
    [ContextMenu("Stop Animation")]
    public void StopAnim()
    {
        ActualAnimStatus = AnimationStatus.AnimationBegin;
    }

    /// <summary>
    /// Stops and play the animation
    /// </summary>
    [ContextMenu("Restart Animation")]
    public void RestartAnim()
    {
        StopAnim();
        PlayAnim();
    }
    
    /// <summary>
    /// Sets the Cam-To-Center distance to value specified in InitZoom field
    /// </summary>
    private void SetInitValues()
    {
        CamCtrl.SetCamToCenterDistance(InitZoom);
    }

    /// <summary>
    /// Runs animation logic
    /// </summary>
    private void RunAnimation()
    {
        // Calcules a ratio in function of Time.time
        float dt = (Time.time - StartTime) / Duration;
        // Calcules the zoom value using the SmoothStep math function
        float value = Mathf.SmoothStep(InitZoom, FinalZoom, dt);
        // Sets the Cam-To-Center value in the camera controller.
        CamCtrl.SetCamToCenterDistance(value);

        if (dt >= 1.0f) // Then animation reaches the end
        {
            if (PingPong) // Executes animation normally and later in reverse if PingPong is equal true
            {
                // Sets animation status to execute in reverse.
                ActualAnimStatus = AnimationStatus.PlayingAnimationInRevere;
                // Saves the time at animation end
                StartTime = Time.time;
            }
            else
                ActualAnimStatus = AnimationStatus.AnimationEnd; // Set animation status to begining
        }
    }

    /// <summary>
    /// Executes animation from end to begin
    /// </summary>
    private void RunReverseAnimation()
    {
        // Calcules a ratio in function of Time.time
        float dt = (Time.time - StartTime) / Duration;
        // Calcules the zoom value using the SmoothStep math function
        float value = Mathf.SmoothStep(FinalZoom, InitZoom, dt);
        // Sets the Cam-To-Center value in the camera controller.
        CamCtrl.SetCamToCenterDistance(value);

        // End of animation if dt>=1
        if (dt >= 1.0f) 
        {
            // Sets the actual animation status to begining
            ActualAnimStatus = AnimationStatus.AnimationBegin;
        }
    }
}
