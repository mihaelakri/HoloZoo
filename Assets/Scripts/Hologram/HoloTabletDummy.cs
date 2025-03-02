using UnityEngine;

public class HoloTabletDummy : MonoBehaviour
{
    public int id_model = 1;

#if UNITY_EDITOR
    void Update()
    {
        if (CommConstants.animal_id != id_model)
        {
            CommConstants.animal_id = id_model;
            StartCoroutine(Load3DModelTablet.GetModel());
        }
    }
#endif
}
