using UnityEngine;

public static class ResizeUtility
{

    public static void ResizeObject(GameObject obj, float initial_size, float targetSize = 2.0f)
    {
        SkinnedMeshRenderer skinnedMeshRenderer = obj.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>();
      
        if (skinnedMeshRenderer != null)
        {
            // Get the bounding box of the temporary object
            Bounds bounds = skinnedMeshRenderer.bounds;

            // Access the size and center of the bounds
            Vector3 boundsSize = bounds.size;
            Vector3 boundsCenter = bounds.center;

            // Print the values to the console
            Debug.Log("Bounds Size: " + boundsSize);
            Debug.Log("Bounds Center: " + boundsCenter);

            // Check if the bounding box has valid size
            if (bounds.size != Vector3.zero)
            {
                // Calculate the largest dimension of the bounding box
                float largestDimension = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

                // Check if the largest dimension is greater than zero
                if (largestDimension > 10)
                {
                    // Calculate the scaling factor to match the target size
                    float scaleFactor = ((largestDimension/2) / largestDimension) * initial_size;

                    // Check for infinity or NaN
                    if (!float.IsInfinity(scaleFactor) && !float.IsNaN(scaleFactor))
                    {
                        // Apply the scaling factor to resize the object
                        obj.transform.localScale *= scaleFactor;
                    }
                    else
                    {
                        Debug.Log("Invalid scale factor. Check the bounding box size and target size.");
                    }
                }
                else if (largestDimension < 4) {
                    // Calculate the scaling factor to match the target size
                    float scaleFactor = (1.5f * largestDimension) * initial_size;

                    // Check for infinity or NaN
                    if (!float.IsInfinity(scaleFactor) && !float.IsNaN(scaleFactor))
                    {
                        // Apply the scaling factor to resize the object
                        obj.transform.localScale *= scaleFactor;
                    }
                    else
                    {
                        Debug.Log("Invalid scale factor. Check the bounding box size and target size.");
                    }

                  }
                else if (largestDimension > 4 && largestDimension < 10)
                {
                    // Calculate the scaling factor to match the target size
                    float scaleFactor = ((largestDimension-1) / largestDimension) * initial_size;

                    // Check for infinity or NaN
                    if (!float.IsInfinity(scaleFactor) && !float.IsNaN(scaleFactor))
                    {
                        // Apply the scaling factor to resize the object
                        obj.transform.localScale *= scaleFactor;
                    }
                    else
                    {
                        Debug.Log("Invalid scale factor. Check the bounding box size and target size.");
                    }

                }
                else
                {
                    Debug.Log("Invalid bounding box size. Ensure the mesh has non-zero dimensions.");
                }
            }
            else
            {
                Debug.Log("Invalid bounding box size. Check the SkinnedMeshRenderer component.");
            }
        }
    }
}


