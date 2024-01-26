using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();

        // Ensure that the Animator component is not null
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    // Function to turn on the animation
    public void TurnOnAnimation(string animationName)
    {
        // Check if the Animator component is not null
        if (animator != null)
        {
            // Set the trigger parameter to start the specified animation
            animator.SetTrigger(animationName);
        }
        else
        {
            Debug.LogError("Animator component is null. Make sure to attach an Animator component to the GameObject.");
        }
    }

    // Function to turn off the animation
    public void TurnOffAnimation(string animationName)
    {
        // Check if the Animator component is not null
        if (animator != null)
        {
            // Set the trigger parameter to stop the specified animation
            animator.ResetTrigger(animationName);
        }
        else
        {
            Debug.LogError("Animator component is null. Make sure to attach an Animator component to the GameObject.");
        }
    }
}
