using UnityEngine;
using UnityEngine.UI;

namespace WPM
{
    public class ShowContinentAnimals : MonoBehaviour
    {
        public GameObject prefabPanel;
        public GameObject prefabAnimal;
        WorldMapGlobe map;
        GameObject currentPanel;
        GameObject currentAnimal;
        public Text contName;

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
            // Instantiate panel if not open
            if (currentPanel == null)
                currentPanel = Instantiate(prefabPanel, GameObject.FindGameObjectWithTag("Content").transform, true);

            // Empty out the panel
            for (int i = 1; i < currentPanel.transform.childCount; i++)
                Destroy(currentPanel.transform.GetChild(i).gameObject);

            int id_continent = GetContinentId(continent);
            string continentTranslated = GameData.Instance.GetArea(id_continent).name;

            contName = currentPanel.GetComponentInChildren<Text>();
            contName.text = continentTranslated;

            Debug.Log($"{nameof(ShowContinentAnimals)} - Continent: {continent}, Translated: {continentTranslated}");
            FillAnimalInfoo(id_continent);
        }

        void FillAnimalInfoo(int id_continent)
        {
            var area = GameData.Instance.GetArea(id_continent);
            var animals = GameData.Instance.GetAreaAnimals(area);

            int x = -255;
            int y = 500;
            int index = 0;

            while (index < animals.Count)
            {
                currentAnimal = Instantiate(prefabAnimal, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("GlobeAnimalPanel").transform);
                currentAnimal.transform.localPosition = new Vector3(x, y, 0);

                // Update panel text and image
                Text animalNamePrefab = currentAnimal.transform.Find("GameObject/Text").GetComponent<Text>();
                Image animalImagePrefab = currentAnimal.transform.Find("GameObject/Image").GetComponent<Image>();

                animalNamePrefab.text = animals[index].name;

                Texture2D myTexture = Resources.Load<Texture2D>(animals[index].url_slika);
                animalImagePrefab.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2());

                if (currentAnimal.TryGetComponent<SceneChange>(out var changeSceneScript))
                    changeSceneScript.id = animals[index].id.ToString();

                x += 250;

                if (index % 3 == 2)
                {
                    y -= 270;
                    x = -235;
                }
                index++;
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