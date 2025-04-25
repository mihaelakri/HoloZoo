using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FillAnimal : MonoBehaviour
{
    public Text nameText;
    public Text generalInfoText;
    public Text endangermentStatusText;
    public Text regionText;
    public Text habitatsText;
    public Text weightText;
    public Text dietText;
    public Text populationText;
    public GameObject animal_photo;
    public ScrollRect scrollRect;

    void Start()
    {
        FillAnimalInfo();
        StartCoroutine(ScrollToTopCoroutine());
    }

    private void FillAnimalInfo()
    {
        var animal = GameData.Instance.GetAnimal(int.Parse(PlayerPrefs.GetString("id_animal")));

        if (animal == null)
        {
            Debug.LogError($"{nameof(FillAnimal)} - Animal is null");
            return;
        }

        nameText.text = animal.name;
        generalInfoText.text = animal.general_info;
        endangermentStatusText.text = animal.endangerment_status;
        habitatsText.text = animal.habitat;
        weightText.text = animal.weight;
        dietText.text = animal.diet;
        populationText.text = animal.population.ToString();

        regionText.text = "";
        for (int i = 0; i < animal.id_area.Count; i++)
        {
            string areaName = GameData.Instance.GetArea(animal.id_area[i]).name;
            regionText.text += $"{areaName}";
            if (i < animal.id_area.Count - 1)
                regionText.text += ", "; ;
        }

        Texture2D myTexture = Resources.Load<Texture2D>(animal.url_slika);
        Image img = animal_photo.AddComponent<Image>();
        img.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2());

        ApplyAccessibility.Instance.ApplyAccessibilitySettings();
    }
    
     IEnumerator ScrollToTopCoroutine()
    {
        yield return new WaitForEndOfFrame();  // Wait for UI to render
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        scrollRect.verticalNormalizedPosition = 1f;
    }
}