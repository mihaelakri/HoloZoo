using UnityEngine;

public class ZoomController : MonoBehaviour
{
    private float minSize = 0.5f;
    private float maxSize = 1.5f;
    private float currentSize = 1.0f;
    private float zoomStep = 0.1f;

    private Transform modelTransform; // Reference to the model's transform

    private void Start()
    {
        currentSize = CommConstants.state.initial_size;
    }

    public void ZoomIn()
    {
        currentSize = Mathf.Min(maxSize, currentSize + zoomStep);
        SetSize(currentSize);
    }

    public void ZoomOut()
    {
        currentSize = Mathf.Max(minSize, currentSize - zoomStep);
        SetSize(currentSize);
    }

    private void SetSize(float size)
    {
        // Find the GameObject with the name "Model"
        GameObject modelObject = GameObject.Find("Model");
        
        // Check if the GameObject is found
        if(modelObject != null)
        {
            // Get the child Transform at index 1
            Transform childTransform = modelObject.transform.GetChild(0);
            
            // Check if the child Transform is valid
            if(childTransform != null)
            {
                // Access the gameObject property of the child Transform
                GameObject child = childTransform.gameObject;

                // Call the ResizeObject function with the GameObject
                child.transform.localScale = new Vector3(size, size, size);
            }
            else
            {
                Debug.LogError("Child transform not found!");
            }
        }
        else
        {
            Debug.LogError("Model GameObject not found!");
        }
    }
}
