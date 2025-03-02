using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load3Dmodel : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetModel());
    }

    public static IEnumerator GetModel()
    {
        var animal = GameData.Instance.GetAnimal(CommConstants.animal_id);
        Debug.Log($"{nameof(Load3Dmodel)} - Animal id: {animal.id}");

        if (SceneManager.GetActiveScene().name == "HologramTablet")
        {
            GameObject parent = GameObject.FindGameObjectWithTag("3d-obj");
            Destroy(parent.transform.GetChild(0).gameObject);
        }

        string model_url = animal.url_model;
        Debug.Log($"{nameof(Load3Dmodel)} - Animal model: {model_url}");
        GameObject variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
        GameObject instantiatedObject = Instantiate(variableForPrefab, new Vector3(0, 2f, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d-obj").transform);

        BoxCollider boxCollider = instantiatedObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(1f, 1f, 1f);
        instantiatedObject.layer = LayerMask.NameToLayer("Animal");
        Rigidbody rb = (Rigidbody)instantiatedObject.AddComponent(typeof(Rigidbody));
        instantiatedObject.GetComponent<Rigidbody>().useGravity = false;
        instantiatedObject.GetComponent<Rigidbody>().isKinematic = true;
        instantiatedObject.GetComponent<Rigidbody>().detectCollisions = false;

        ResizeUtility.ResizeObject(instantiatedObject);

        yield break;
    }
}