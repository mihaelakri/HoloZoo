using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate : MonoBehaviour
{
    //Mouse 

    private float _sensitivity;
	private Vector3 _mouseReference;
	private Vector3 _mouseOffset;
	private Vector3 _rotation;
	private bool _isRotating;

    // Start is called before the first frame update
    void Start()
    {
        _sensitivity = 0.4f;
		_rotation = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        if(_isRotating)
        {
            _mouseOffset = (Input.mousePosition - _mouseReference);

        // apply rotation, considering a 360 degree range
        _rotation.y = -(_mouseOffset.x) * _sensitivity * 360.0f / Screen.width;
        _rotation.x = _mouseOffset.y * _sensitivity * 360.0f / Screen.height;

        // rotate
        transform.Rotate(_rotation);

        // Update CommConstants after each rotation change, rounding to the nearest integer within 360 range
        CommConstants.x = Mathf.RoundToInt((transform.eulerAngles.x + 360.0f) % 360.0f);
        CommConstants.y = Mathf.RoundToInt((transform.eulerAngles.y + 360.0f) % 360.0f);
        CommConstants.z = Mathf.RoundToInt((transform.eulerAngles.z + 360.0f) % 360.0f);

        // store mouse
        _mouseReference = Input.mousePosition;
        }
    }

    void OnMouseDown()
	{
		// rotating flag
		_isRotating = true;
		
		// store mouse
		_mouseReference = Input.mousePosition;
	}
	
	void OnMouseUp()
	{
		// rotating flag
		_isRotating = false;
	}
}
