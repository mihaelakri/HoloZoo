using System;
using System.Collections;
using UnityEngine;

public class Load3DModelTablet : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetModel());
    }

    public static IEnumerator GetModel()
    {
        var animal = GameData.Instance.GetAnimal(CommConstants.animal_id);
        Debug.Log($"{nameof(Load3Dmodel)} - Animal id: {animal.id}");

        string model_url;
        if (CommConstants.animal_id == 0)
            model_url = "WorldMapGlobe";
        else
            model_url = animal.url_model;

        Debug.Log($"{nameof(Load3Dmodel)} - Animal model: {model_url}");

        GameObject parent = GameObject.FindGameObjectWithTag("3d-obj");
        if (parent.transform.childCount > 0)
        {
            // Destroy(parent.transform.GetChild(0).gameObject);
            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        model_url = "AnimalModels/" + model_url.Split('/')[^1];
        Debug.Log($"Transformed model_url: {model_url}");

        GameObject variableForPrefab = (GameObject)Resources.Load(model_url, typeof(GameObject));
        GameObject instantiatedObject = Instantiate(variableForPrefab, new Vector3(0, -1, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d-obj").transform);
        //Instantiate(variableForPrefab, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("3d-obj").transform);

        Animator animator = instantiatedObject.GetComponent<Animator>();
        animator.Play("IdleBreathe");
        animator.StopPlayback();

        if (model_url == "WorldMapGlobe")
        {
            yield break;
        }

        BoxCollider boxCollider = instantiatedObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(1f, 1f, 1f);
        instantiatedObject.layer = LayerMask.NameToLayer("Animal");
        Rigidbody rb = (Rigidbody)instantiatedObject.gameObject.AddComponent(typeof(Rigidbody));
        ResizeUtility.ResizeObjectTablet(instantiatedObject);
        // instantiatedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        instantiatedObject.GetComponent<Rigidbody>().useGravity = false;
        instantiatedObject.GetComponent<Rigidbody>().isKinematic = true;
        instantiatedObject.GetComponent<Rigidbody>().detectCollisions = false;

        yield break;
    }
}