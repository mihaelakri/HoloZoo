using UnityEngine;

public class HoloTabletDummy : MonoBehaviour
{
    [SerializeField, Range(0, 20)]
    int id_model = 1;
    Load3DModelTablet load3DModelTablet;

#if UNITY_EDITOR
    void Start()
    {
        load3DModelTablet = GameObject.Find("Canvas").GetComponent<Load3DModelTablet>();
        if (load3DModelTablet == null)
        {
            Debug.LogError($"{nameof(HoloTabletDummy)} - Couldn't find {nameof(Load3DModelTablet)}");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (CommConstants.animal_id != id_model)
        {
            CommConstants.animal_id = id_model;
            StartCoroutine(load3DModelTablet.GetModel());
        }
    }
#endif
}
