using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FillLearnAnimals : MonoBehaviour
{
    public GameObject list_element;

    void Start()
    {
        if (PlayerPrefs.HasKey("ID"))
        {
            StartCoroutine(FillAnimals());
        }
    }

    IEnumerator FillAnimals()
    {
        int level = GameData.Instance.GetCurrentUser().level;
        var animals = GameData.Instance.GetAnimalNames(level);

        foreach (var animal in animals)
        {
            GameObject newobj = Instantiate(list_element);
            newobj.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform, false);
            Text newText = newobj.GetComponentInChildren<Text>();
            newText.text = animal.name;
            newobj.name = animal.id.ToString();
        }
        GameObject.Find("AccessibilityManager").GetComponent<ApplyAccessibility>().LoadAndStyle();

        yield break;
    }
}