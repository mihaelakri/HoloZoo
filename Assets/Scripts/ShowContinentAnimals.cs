using UnityEngine;
using UnityEngine.UI;

namespace WPM
{
    public class ShowContinentAnimals : MonoBehaviour
    {
        [SerializeField]
        GameObject animalPanel, animalContainer, prefabAnimal, prefabRow;

        [SerializeField]
        Text continentName;
        WorldMapGlobe map;

        void Start()
        {
            // Get a reference to the World Map API:
            map = WorldMapGlobe.instance;
            map.OnContinentClick += (continent, buttonIndex) => AddPanel(continent, buttonIndex);

            map.OnClick += (sphereLocation, mouseButtonIndex) =>
            {
                if (!map.GetCountryUnderSpherePosition(sphereLocation, out int countryIndex, out int regionIndex))
                {
                    // Debug.Log($"{nameof(ShowContinentAnimals)} - Country not found at sphereLocation: {sphereLocation}");
                    AddPanel("Oceans", 0);
                }
            };
        }

        void AddPanel(string continent, int buttonIndex)
        {
            // Enable panel
            animalPanel.SetActive(true);

            // Empty out the panel
            for (int i = 1; i < animalContainer.transform.childCount; i++)
                Destroy(animalContainer.transform.GetChild(i).gameObject);

            int id_continent = GetContinentId(continent);
            string continentTranslated = GameData.Instance.GetArea(id_continent).name;
            continentName.text = continentTranslated;

            Debug.Log($"{nameof(ShowContinentAnimals)} - Continent: {continent}, Translated: {continentTranslated}");
            FillAnimalInfoo(id_continent);
            ApplyAccessibility.Instance.LoadAndStyle();
        }

        void FillAnimalInfoo(int id_continent)
        {
            var area = GameData.Instance.GetArea(id_continent);
            int userLevel = GameData.Instance.GetCurrentUser()?.level ?? 1;
            if (area == null)
                Debug.LogError($"{nameof(ShowContinentAnimals)} - area not found, id_continent: {id_continent}");
            var animals = GameData.Instance.GetAreaAnimals(area, userLevel);

            int rowCounter = 0;
            GameObject row = Instantiate(prefabRow, animalContainer.transform);

            foreach (var animal in animals)
            {
                if (rowCounter >= 3)
                {
                    row = Instantiate(prefabRow, animalContainer.transform);
                    rowCounter = 0;
                }
                var currentAnimal = Instantiate(prefabAnimal, row.transform);

                // Update panel text and image
                Text animalNamePrefab = currentAnimal.transform.GetComponentInChildren<Text>();
                Image animalImagePrefab = currentAnimal.transform.GetComponentInChildren<Image>();

                animalNamePrefab.text = animal.name;

                Texture2D myTexture = Resources.Load<Texture2D>(animal.url_slika);
                animalImagePrefab.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2());

                if (currentAnimal.TryGetComponent<SceneChange>(out var changeSceneScript))
                    changeSceneScript.id = animal.id.ToString();

                rowCounter++;
            }
        }

        public int GetContinentId(string continentName)
        {
            switch (continentName)
            {
                case "Africa":
                    return 1;
                case "Europe":
                    return 2;
                case "Asia":
                    return 3;
                case "Australia":
                    return 4;
                case "Eurasia":
                    Debug.LogWarning($"{nameof(ShowContinentAnimals)} - continent Eurasia shouldn't be available, but is selected.");
                    return 5;
                case "Antarctica":
                    return 6;
                case "South America":
                    return 7;
                case "North America":
                    return 8;
                case "Oceans":
                    return 9;
                default:
                    Debug.LogError($"{nameof(ShowContinentAnimals)} - GetContinentId failed to find a continent with name '{continentName}'");
                    return 0;
            }
        }
    }
}