using UnityEngine;

/// <summary>
/// Helper Script for animate the pyramidal camera position
/// </summary>
/// 
/// <remarks>
/// This is not the only way of animate the camera position, 
/// users can implement his own way of achieve the same.
/// 
/// NOTE: This script can be atached to any GameObject which have a Transform component.
/// 
/// We would like to listen your ideas about this topic, please send us a email to: gamebite10@gmail.com
/// </remarks>
[RequireComponent(typeof(Transform))]
public class HoloCamPositionAnim : MonoBehaviour
{
    /// <summary>
    /// Initial position for the animation
    /// </summary>
    public Vector3 InitPosition;
    /// <summary>
    /// Final position for the animation
    /// </summary>
    public Vector3 FinalPosition;
    /// <summary>
    /// Animation duratin from start to end (If PingPong is true duration will be duplicated)
    /// </summary>
    [Tooltip("Animation duration in seconds")]
    public float Duration = 3.0f;
    /// <summary>
    /// Executes animation in reverse when reaches the end status: begin->end->begin
    /// </summary>
    [Tooltip("Execute animation normally and on his end execute it in reverse. If PingPong is enabled then animation duration will be: Duration * 2")]
    public bool PingPong = false;

    private AnimationStatus AnimStatus = AnimationStatus.AnimationBegin;
    private float StartTime;
    private Transform CamTransf;
    private Vector3 ActualPosition;

    // Use this for initialization
    void Start () {
        // At begining set InitPosition as the ActualPosition
        ActualPosition = InitPosition;
        // Saves time at initialization
        StartTime = Time.time;
        // Gets a reference to Transform component atached to this game object
        CamTransf = gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        // The animation is controlled by a basic state machine
        switch (AnimStatus)
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
    /// Starts the animation
    /// NOTE: Animation can start only when AnimationStatus is in begin or end.
    /// </summary>
    [ContextMenu("Play Animation")]
    public void PlayAnim()
    {
        if (AnimStatus == AnimationStatus.AnimationBegin || AnimStatus == AnimationStatus.AnimationEnd)
        {
            AnimStatus = AnimationStatus.PlayingAnimation;
            StartTime = Time.time;
        }

    }

    /// <summary>
    /// Stops and set animation status to the beginning
    /// User can stop animation in any moment.
    /// </summary>
    [ContextMenu("Stop Animation")]
    public void StopAnim()
    {
        AnimStatus = AnimationStatus.AnimationBegin;
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
    /// Sets transform position to value specified in InitPosition field
    /// </summary>
    private void SetInitValues()
    {
        ActualPosition = InitPosition;
        CamTransf.position = ActualPosition;
    }

    /// <summary>
    /// Runs animation logic
    /// </summary>
    private void RunAnimation()
    {
        // Calcules a ratio in function of Time.time
        float dt = (Time.time - StartTime) / Duration;
        // Calcules the newX value for actual position using SmoothStep function
        float NewX = Mathf.SmoothStep(InitPosition.x, FinalPosition.x, dt);
        // Calcules the newY value for actual position using SmoothStep function
        float NewY = Mathf.SmoothStep(InitPosition.y, FinalPosition.y, dt);
        // Calcules the newZ value for actual position using SmoothStep function
        float NewZ = Mathf.SmoothStep(InitPosition.z, FinalPosition.z, dt);
        // Sets to ActualPosition vector the values calcalated previously
        ActualPosition.Set(NewX, NewY, NewZ);

        // Sets ActualPosition vector to gameOnject's transform position 
        CamTransf.position = ActualPosition;

        if (dt >= 1.0f) // If true is because the animation reaches the end
        {
            if (PingPong) // Executes animation normally and later in reverse if PingPong is equal true
            {
                // Sets animation status to execute in reverse.
                AnimStatus = AnimationStatus.PlayingAnimationInRevere;
                // Saves the time at animation end
                StartTime = Time.time;
            }
            else
                AnimStatus = AnimationStatus.AnimationEnd; // Sets animation status to begining
        }
    }

    /// <summary>
    /// Executes animation from end to begin
    /// </summary>
    private void RunReverseAnimation()
    {
        // Computes a ratio in function of Time.time
        float dt = (Time.time - StartTime) / Duration;
        // Computes the newX value for actual position using SmoothStep function
        float NewX = Mathf.SmoothStep(FinalPosition.x, InitPosition.x, dt);
        // Computes the newY value for actual position using SmoothStep function
        float NewY = Mathf.SmoothStep(FinalPosition.y, InitPosition.y, dt);
        // Sets to ActualPosition vector the values calcalated previously
        float NewZ = Mathf.SmoothStep(FinalPosition.z, InitPosition.z, dt);
        // Sets ActualPosition vector to gameOnject's transform position 
        ActualPosition.Set(NewX, NewY, NewZ);

        // Sets ActualPosition vector to gameOnject's transform position 
        CamTransf.position = ActualPosition;

        if (dt >= 1.0f) // If true is because the animation reaches the end
        {
            // Sets animation status to begining
            AnimStatus = AnimationStatus.AnimationBegin;
        }
    }

}
