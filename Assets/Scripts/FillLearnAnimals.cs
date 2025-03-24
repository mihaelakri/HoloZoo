using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FillLearnAnimals : MonoBehaviour
{
    [SerializeField]
    GameObject list_element, lockedAnimalElement;

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
        var lockedAnimals = GameData.Instance.GetLockedAnimalNames(level);

        foreach (var animal in animals)
        {
            GameObject newobj = Instantiate(list_element);
            newobj.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform, false);
            Text newText = newobj.GetComponentInChildren<Text>();
            newText.text = animal.name;
            newobj.name = animal.id.ToString();
        }
        foreach (var lockedAnimal in lockedAnimals)
        {
            GameObject newElement = Instantiate(lockedAnimalElement);
            newElement.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform, false);
            var texts = newElement.GetComponentsInChildren<Text>();
            texts[0].text = lockedAnimal.name;
            texts[1].text = $"lvl {lockedAnimal.level}";
            newElement.name = lockedAnimal.id.ToString();
        }
        ApplyAccessibility.Instance.LoadAndStyle();

        yield break;
    }
}