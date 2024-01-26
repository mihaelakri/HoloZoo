using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateRight() {
        CommConstants.y += 25;
    }
    public void RotateLeft()
    {
        CommConstants.y -= 25;
    }

    public void RotateUp()
    {
        CommConstants.x += 25;
    }

    public void RotateDown()
    {
        CommConstants.x -= 25;
    }
}
