using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace WPM {
    public class ShowContinentAnimals : MonoBehaviour
    {
        public GameObject prefabPanel;
        public GameObject prefabAnimal;
        public string contNameGlobe; 
        WorldMapGlobe map;
        GameObject currentPanel;
        GameObject currentAnimal;
        int counter=0;

        [Serializable]
        public class Animal{
            public String name;
            public String url_slika;
        }

        public Animals animals;

        [Serializable]
        public class Animals{
            public Animal[] animal;
        }

        public Text contName;


        void Start() {
                // Get a reference to the World Map API:
                map = WorldMapGlobe.instance;
                map.OnContinentClick += (string continent, int buttonIndex) => AddPanel(map.countryHighlighted.continent);
                
                
        }
        
        void AddPanel(string continent) {
                GameObject[] allAnimalPrefabs;

                allAnimalPrefabs = GameObject.FindGameObjectsWithTag("AnimalPrefab(Clone)");

                foreach(GameObject animprefab in allAnimalPrefabs)
                {
                    Destroy(animprefab);
                }

                // If previous panel exists, destroy it
                if (currentPanel != null) {
                    Destroy(currentPanel);
                }

                // Instantiate panel
                currentPanel = Instantiate<GameObject>(prefabPanel);
                contName = currentPanel.GetComponentInChildren<Text>();
                contName.text = continent;

                contNameGlobe = map.countryHighlighted.continent;

                StartCoroutine(FillAnimalInfoo());
        }

        IEnumerator FillAnimalInfoo(){

        WWWForm form = new WWWForm();
        form.AddField("id_area", 1);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/HoloZoo/animal_view.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {

                    animals = JsonUtility.FromJson<Animals>(www.downloadHandler.text);
                    var x = -115; 
                    var y = 210;
                    counter=1;
                    
                        foreach (var a in animals.animal){
                            currentAnimal = Instantiate<GameObject>(prefabAnimal, currentPanel.transform);
                            
                            // 3 po 3 imaju isti y ali razliciti x
                            // isti x imaju svaki treci
                            
                            currentAnimal.transform.localPosition = new Vector3(x, y, 0);

                            x += 115;
                            
                            if(counter % 3== 0){
                                y-=140;
                                x=-115;
                            }
                            counter += 1;

                            // Update panel text and image
                            Text animalNamePrefab;
                            Image animalImagePrefab;
                            
                            animalNamePrefab = currentAnimal.transform.Find("GameObject/Text").GetComponent<Text>();
                            animalImagePrefab = currentAnimal.transform.Find("GameObject/Image").GetComponent<Image>();

                            animalNamePrefab.text = a.name;

                            UnityWebRequest request = UnityWebRequestTexture.GetTexture("http://localhost/HoloZoo/" + a.url_slika);
                            yield return request.SendWebRequest();

                            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                            animalImagePrefab.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2());
                        }
                    
                }
        }
    }
    }
}