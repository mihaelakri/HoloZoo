using System.Collections;
using UnityEngine;

public class Load3DModelTablet : MonoBehaviour
{
    public Camera referenceCamera;

    void Start()
    {
        StartCoroutine(GetModel());
    }

    public IEnumerator GetModel()
    {
        var animal = GameData.Instance.GetAnimal(CommConstants.animal_id);
        string model_url;

        if (CommConstants.animal_id == 0)
            model_url = "WorldMapGlobe";
        else
            model_url = "AnimalModels/" + animal.url_model.Split('/')[^1];

        GameObject parent = GameObject.FindGameObjectWithTag("3d-obj");
        if (parent.transform.childCount > 0)
        {
            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        GameObject modelPrefab = Resources.Load<GameObject>(model_url);

        if (model_url == "WorldMapGlobe")
        {
            GameObject instantiatedObject = Instantiate(modelPrefab, new Vector3(0, 0, 0), Quaternion.identity, parent.transform);
            ResizeUtility.ScaleObjectToFitCamera(instantiatedObject, instantiatedObject.GetComponentInChildren<MeshRenderer>(), referenceCamera, 0.8f);
        }
        else
        {
            GameObject instantiatedObject = Instantiate(modelPrefab, new Vector3(0, 0, 0), Quaternion.identity, parent.transform);
            ResizeUtility.ScaleObjectToFitCamera(instantiatedObject, instantiatedObject.GetComponentInChildren<SkinnedMeshRenderer>(), referenceCamera);
            ResizeUtility.CenterObjectVertically3(instantiatedObject, instantiatedObject.GetComponentInChildren<SkinnedMeshRenderer>(), referenceCamera);
            ResizeUtility.CreateCenterPivot(instantiatedObject, parent.transform);
        }

        yield break;
    }
}