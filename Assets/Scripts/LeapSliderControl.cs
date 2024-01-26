using UnityEngine;
using UnityEngine.UI;
using Leap;

public class LeapSliderControl : MonoBehaviour
{
    public Slider mySlider;  // Reference to your UI Slider
    private Controller leapController;

    private bool isHandInteractingWithSlider = false;

    void Start()
    {
        leapController = new Controller();
    }

    void Update()
    {
        Frame frame = leapController.Frame();

        if (frame.Hands.Count > 0)
        {
            Hand firstHand = frame.Hands[0];

            // Check if the hand is close to the slider
            CheckHandProximityToSlider(firstHand);

            if (isHandInteractingWithSlider)
            {
                // Update slider value based on hand position
                UpdateSlider(firstHand.PalmPosition.y);
            }
            else
            {
                // Rotate object based on hand movement
                RotateObject(firstHand);
            }
        }
    }

    void RotateObject(Hand hand)
    {
        // Implement rotation logic based on hand movement
        // For example, you can use hand.Rotation or hand.Direction
        // to determine the rotation of your object.
        // ...

        // Sample code (rotate object around Y-axis):
        float rotationSpeed = 5.0f;
        transform.Rotate(Vector3.up, hand.PalmVelocity.x * rotationSpeed * Time.deltaTime);
    }

    void UpdateSlider(float handPositionY)
    {
        // Map hand position to slider value
        float normalizedHandPosition = Mathf.Clamp01((handPositionY - 100) / 200); // Adjust range as needed
        float sliderValue = Mathf.Lerp(mySlider.minValue, mySlider.maxValue, normalizedHandPosition);

        // Update the slider value
        mySlider.value = sliderValue;
    }

    void CheckHandProximityToSlider(Hand hand)
    {
        // Implement logic to check if the hand is close to the slider
        // For example, you can use the position of the slider and the hand
        // to determine if they are close enough for interaction.
        // ...

        // Sample code (check if hand is within a certain range in the Y-axis):
        float sliderY = mySlider.transform.position.y;
        float handY = hand.PalmPosition.y;

        float proximityThreshold = 50.0f; // Adjust as needed

        isHandInteractingWithSlider = Mathf.Abs(handY - sliderY) < proximityThreshold;
    }
}
