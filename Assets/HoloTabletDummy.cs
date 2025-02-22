using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloTabletDummy : MonoBehaviour
{
    public int id_model = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CommConstants.animal_id != id_model)
        {
            CommConstants.animal_id = id_model;
            StartCoroutine(Load3DModelTablet.GetModel());
        }
    }
}
