using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load3Dmodel : MonoBehaviour
{
    public Camera referenceCamera;

    void Start()
    {
        StartCoroutine(GetModel());
    }

    public IEnumerator GetModel()
    {
        var animal = GameData.Instance.GetAnimal(CommConstants.animal_id);
        // Debug.Log($"{nameof(Load3Dmodel)} - Animal id: {animal.id}");
        string model_url = animal.url_model;

        if (SceneManager.GetActiveScene().name == "HologramTablet")
        {
            GameObject parent = GameObject.FindGameObjectWithTag("3d-obj");
            Destroy(parent.transform.GetChild(0).gameObject);
        }

        GameObject variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
        GameObject instantiatedObject = Instantiate(variableForPrefab, new Vector3(0, 2f, 0), Quaternion.Euler(0, 180f, 0), GameObject.FindGameObjectWithTag("3d-obj").transform);

        ResizeUtility.ScaleObjectToFitCamera(instantiatedObject, instantiatedObject.GetComponentInChildren<SkinnedMeshRenderer>(), referenceCamera);
        ResizeUtility.CenterObjectVertically3(instantiatedObject, instantiatedObject.GetComponentInChildren<SkinnedMeshRenderer>(), referenceCamera);

        yield break;
    }
}