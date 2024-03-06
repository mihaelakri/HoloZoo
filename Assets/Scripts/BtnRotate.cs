using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnRotate : MonoBehaviour
{

    public void RotateRight() {
        CommConstants.state.y += 25;
    }
    public void RotateLeft()
    {
        CommConstants.state.y -= 25;
    }

    public void RotateUp()
    {
        CommConstants.state.x += 25;
      
    }

    public void RotateDown()
    {
        CommConstants.state.x -= 25;
    }
}
