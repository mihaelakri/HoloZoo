using UnityEngine;

public static class ResizeUtility
{
    public static void ResizeObject(GameObject obj)
    {
        // LODGroup lodGroup = obj.GetComponent<LODGroup>();
        // Debug.Log("LODGroup size: " + lodGroup.GetLODs()[0].renderers[0].bounds.size);

        SkinnedMeshRenderer skinnedMeshRenderer = obj.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>();

        if (skinnedMeshRenderer != null)
        {
            // Get the bounding box of the temporary object
            Bounds bounds = skinnedMeshRenderer.bounds;

            // Access the size and center of the bounds
            Vector3 boundsSize = bounds.size;

            // Print the values to the console
            Debug.Log("Bounds Size: " + boundsSize);

            // Check if the bounding box has valid size
            if (bounds.size != Vector3.zero)
            {
                // Calculate the largest dimension of the bounding box
                float largestDimension = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

                // Check if the largest dimension is greater than zero
                if (largestDimension > 10)
                {
                    // Calculate the scaling factor to match the target size
                    float scaleFactor = ((largestDimension / 2) / largestDimension);

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
                else if (largestDimension < 4)
                {
                    // Calculate the scaling factor to match the target size
                    float scaleFactor = (3f * largestDimension);

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
                    float scaleFactor = ((largestDimension - 1) / largestDimension);

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
                Debug.Log("Object Resized bounds: " + boundsSize);
            }
            else
            {
                Debug.Log("Invalid bounding box size. Check the SkinnedMeshRenderer component.");
            }
        }
    }

    public static void ResizeObjectTablet(GameObject obj)
    {
        // LODGroup lodGroup = obj.GetComponent<LODGroup>();
        // Debug.Log("LODGroup size: " + lodGroup.GetLODs()[0].renderers[0].bounds.size);

        SkinnedMeshRenderer skinnedMeshRenderer = obj.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>();

        if (skinnedMeshRenderer != null)
        {
            // Get the bounding box of the temporary object
            Bounds bounds = skinnedMeshRenderer.bounds;

            // Access the size and center of the bounds
            Vector3 boundsSize = bounds.size;

            // Print the values to the console
            Debug.Log("Bounds Size: " + boundsSize);

            // Check if the bounding box has valid size
            if (bounds.size != Vector3.zero)
            {
                // Calculate the largest dimension of the bounding box
                float largestDimension = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

                // Check if the largest dimension is greater than zero
                if (largestDimension > 5.5)
                {
                    // Calculate the scaling factor to match the target size
                    float scaleFactor = ((largestDimension / 2) / largestDimension) * 0.75f;

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
                else if (largestDimension < 1.5)
                {
                    // Calculate the scaling factor to match the target size
                    float scaleFactor = (6f * largestDimension);

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
                else if (largestDimension >= 1.5 && largestDimension < 3)
                {
                    // Calculate the scaling factor to match the target size
                    float scaleFactor = ((largestDimension - 1) / largestDimension) * 1.5f;

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
                else if (largestDimension >= 3 && largestDimension < 5.5)
                {
                    // Calculate the scaling factor to match the target size
                    float scaleFactor = ((largestDimension - 1) / largestDimension) * 1.2f;

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
                Debug.Log("Object Resized bounds: " + boundsSize);
            }
            else
            {
                Debug.Log("Invalid bounding box size. Check the SkinnedMeshRenderer component.");
            }
        }
    }
}


