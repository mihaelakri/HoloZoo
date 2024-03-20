using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnRotate : MonoBehaviour
{

    public void RotateRight()
    {
        CommConstants.rotation.y += 25;
        CommConstants.rotation.y += 25;
    }
    public void RotateLeft()
    {
        CommConstants.rotation.y -= 25;
        CommConstants.rotation.y -= 25;
    }

    public void RotateUp()
    {
        CommConstants.rotation.x += 25;
        CommConstants.rotation.x += 25;

    }

    public void RotateDown()
    {
        CommConstants.rotation.x -= 25;
        CommConstants.rotation.x -= 25;
    }
}
