using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnRotate : MonoBehaviour
{

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
