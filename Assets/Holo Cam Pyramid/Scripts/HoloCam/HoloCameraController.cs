using UnityEngine;

/// <summary>
/// Controls Holographic Cameras.
/// </summary>
public class HoloCameraController : MonoBehaviour {
    
    /// <summary>
    /// Reflects the distance between each pyramid camera and the local center
    /// </summary>
    [HideInInspector]
    public float cameraToCenterDistance;

    /// <summary>
    /// Sets/Gets the cameraToCenterDistance field
    /// </summary>
    public float CamToCenter
    {
        get
        {
            return cameraToCenterDistance;
        }

        set
        {
            SetCamToCenterDistance(value);
        }
    }

    // Directions are given supposing that front is Z axis positive arrow
    /// <summary>
    /// Pyramid Left Camera Transform component reference
    /// </summary>
    public Transform LeftCamera;
    /// <summary>
    /// Pyramid Right Camera Transform component reference
    /// </summary>
    public Transform RightCamera;
    /// <summary>
    /// Pyramid Back Camera Transform component reference
    /// </summary>
    public Transform BackCamera;
    /// <summary>
    /// Pyramid Front Camera Transform component reference
    /// </summary>
    public Transform FrontCamera;

    private Vector3 newPos;

	// Use this for initialization
	void Start () {
    }

    /// <summary>
    /// Sets the distance from each pyramid camera to the local center, this method can be understood as a 
    /// zoom controller.
    /// </summary>
    /// <param name="distToCenter">Distance from each camera to local center</param>
    public void SetCamToCenterDistance(float distToCenter)
    {
        //Debug.Log("Distancia  center " + distToCenter);
        newPos = new Vector3(0, 0, 0);

        // Updates cameraToCenterDistance field.
        cameraToCenterDistance = distToCenter;

        // Gets a copy of left camera position vector 
        newPos = LeftCamera.position;
        // Calcules the X component of new left camera position relative to parent gameobject position
        newPos.x = -1 * (distToCenter - gameObject.transform.position.x);
        // Assigns previous calculated position to left camera position
        LeftCamera.position = newPos;

        // Gets a copy of right camera position vector
        newPos = RightCamera.position;
        // Calcules the X component of new right camera position relative to parent gameobject position
        newPos.x = (distToCenter + gameObject.transform.position.x);
        // Assigns previous calculated position to right camera position
        RightCamera.position = newPos;

        // Gets a copy of back camera position vector
        newPos = BackCamera.position;
        // Calcules the Z component of new back camera position relative to parent gameobject position
        newPos.z = (distToCenter + gameObject.transform.position.z);
        // Assigns previous calculated position to back camera position
        BackCamera.position = newPos;

        // Gets a copy of front camera position vector
        newPos = FrontCamera.position;
        // Calcules the Z component of new fromt camera position relative to parent gameobject position
        newPos.z = -1 * (distToCenter - gameObject.transform.position.z);
        // Assigns previous calculated position to front camera position
        FrontCamera.position = newPos;
    }

	// Update is called once per frame
	void Update () {
    }
}
