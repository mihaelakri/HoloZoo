using UnityEngine;

public static class ResizeUtility
{
    public static void ScaleObjectToFitCamera(GameObject targetObject, Renderer referenceRenderer, Camera referenceCamera, float paddingPercent = 0.95f)
    {
        if (targetObject == null || referenceRenderer == null || referenceCamera == null)
        {
            Debug.LogError($"{nameof(Load3DModelTablet)}, ScaleObjectToFitCamera - Something is null: " +
                $"targetObject: {targetObject}, referenceRenderer: {referenceRenderer}, referenceCamera: {referenceCamera}");
            return;
        }

        // Calculate the object's local bounds
        Bounds localBounds = referenceRenderer.bounds;
        Vector3 objectSize = localBounds.size;

        // Calculate distance to object
        Vector3 cameraToObject = targetObject.transform.position - referenceCamera.transform.position;
        float distanceToObject = cameraToObject.magnitude;

        // Calculate the camera's view size at the specified distance
        float cameraHeight = 2.0f * distanceToObject * Mathf.Tan(referenceCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float cameraWidth = cameraHeight * referenceCamera.aspect;

        // Find the largest axis of the object's bounding box
        float maxObjectAxis = Mathf.Max(objectSize.x, objectSize.y, objectSize.z);

        // Find the smallest camera axis
        float minCameraAxis = Mathf.Min(cameraWidth, cameraHeight);

        // Calculate the scale factor
        float scaleFactor = (minCameraAxis / maxObjectAxis) * paddingPercent;

        // Apply the scale
        targetObject.transform.localScale = Vector3.one * scaleFactor;
    }

    public static void CenterObjectVertically3(GameObject targetObject, Renderer referenceRenderer, Camera referenceCamera)
    {
        if (targetObject == null || referenceRenderer == null || referenceCamera == null)
        {
            Debug.LogError($"{nameof(Load3DModelTablet)}, ScaleObjectToFitCamera - Something is null: " +
                $"targetObject: {targetObject}, referenceRenderer: {referenceRenderer}, referenceCamera: {referenceCamera}");
            return;
        }

        // Get the world-space center of the referenceRenderer
        Bounds bounds = referenceRenderer.bounds;
        float objectVerticalCenter = bounds.center.y;

        // Get the world-space vertical center of the camera
        Vector3 cameraCenter = referenceCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, referenceCamera.nearClipPlane));
        float cameraVerticalCenter = cameraCenter.y;

        // Compute the vertical offset
        float verticalOffset = cameraVerticalCenter - objectVerticalCenter;

        // Apply the offset to the targetObject
        targetObject.transform.position += new Vector3(0, verticalOffset, 0);
    }
}