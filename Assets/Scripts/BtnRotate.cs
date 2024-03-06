using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnRotate : MonoBehaviour
{

    public void RotateRight() {
        CommConstants.rotationMsg.y += 25;
    }
    public void RotateLeft()
    {
        CommConstants.rotationMsg.y -= 25;
    }

    public void RotateUp()
    {
        CommConstants.rotationMsg.x += 25;
      
    }

    public void RotateDown()
    {
        CommConstants.rotationMsg.x -= 25;
    }
}
