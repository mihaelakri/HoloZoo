using UnityEngine;
using Leap;
using Leap.Unity;

public class RotationLeapMotion : MonoBehaviour
{
    public bool rotateEnabled = true;
    public GameObject objectToRotate; // Assign the object you want to rotate in the Unity Inspector
    public float rotationSpeed = 1f;

    private LeapServiceProvider leapProvider;

    void Start()
    {
        LoadRandomModel.OnModelLoaded += RotateModel;
        leapProvider = FindObjectOfType<LeapServiceProvider>();

        if (leapProvider == null)
        {
            Debug.LogError("LeapServiceProvider not found in the scene.");
        }
    }

    void RotateModel()
    {
        if (rotateEnabled && objectToRotate != null && leapProvider != null)
        {
            Frame frame = leapProvider.CurrentFrame;

            if (frame != null && frame.Hands.Count > 0)
            {
                Vector3 averagePalmDirection = Vector3.zero;

                foreach (Hand hand in frame.Hands)
                {
                    // Accumulate palm directions of all hands
                    averagePalmDirection += hand.PalmNormal;
                }

                // Calculate average palm direction
                averagePalmDirection /= frame.Hands.Count;

                // Create a rotation based on the average hand direction
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, averagePalmDirection);

                // Apply rotation to the object directly
                objectToRotate.transform.rotation = Quaternion.Slerp(objectToRotate.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
