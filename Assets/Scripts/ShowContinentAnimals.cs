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
            public string id_animal;
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
                int id_continent;

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
                currentPanel.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
                contName = currentPanel.GetComponentInChildren<Text>();
                contName.text = continent;
                id_continent = getContinentId(continent);

                contNameGlobe = map.countryHighlighted.continent;

                StartCoroutine(FillAnimalInfoo(id_continent));
        }

        IEnumerator FillAnimalInfoo(int id_continent){

        WWWForm form = new WWWForm();
        form.AddField("id_area", id_continent);
        Debug.Log(id_continent);

        //int id_area = id_continent;

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"animal_view.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    Debug.Log(www.downloadHandler.text);
                    animals = JsonUtility.FromJson<Animals>(www.downloadHandler.text);
                    var x = -255; 
                    var y = 530;
                    counter=1;
                    
                        foreach (var a in animals.animal){
                            Debug.Log(a.name);
                            currentAnimal = Instantiate(prefabAnimal, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("oblacic").transform);
                            
                            // 3 po 3 imaju isti y ali razliciti x
                            // isti x imaju svaki treci
                            
                            currentAnimal.transform.localPosition = new Vector3(x, y, 0);

                            x += 250;
                            
                            if(counter % 3== 0){
                                y-=250;
                                x=-235;
                            }
                            counter += 1;

                            // Update panel text and image
                            Text animalNamePrefab;
                            Image animalImagePrefab;
                            
                            animalNamePrefab = currentAnimal.transform.Find("GameObject/Text").GetComponent<Text>();
                            animalImagePrefab = currentAnimal.transform.Find("GameObject/Image").GetComponent<Image>();

                            animalNamePrefab.text = a.name;

                            UnityWebRequest request = UnityWebRequestTexture.GetTexture(CommConstants.ServerURL + a.url_slika);
                            yield return request.SendWebRequest();

                            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                            animalImagePrefab.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2());
                       
                            ChangeSceneGlobe changeSceneScript = currentAnimal.GetComponent<ChangeSceneGlobe>();
                            if (changeSceneScript != null)
                                {
                                    changeSceneScript.id = a.id_animal;
                                }
                        }
                    
                }
        }
    }
    public int getContinentId(string contName){
        if(contName=="Africa"){
            return 1; 
        }
        else if(contName=="Europe"){
            return 2;
        }
        else if(contName=="Asia"){
            return 3;
        }
        else if(contName=="Australia"){
            return 4;
        }
        else if(contName=="Eurasia"){
            return 5;
        }
        else if(contName=="Antarctica"){
            return 6;
        }
        else if(contName=="South America"){
            return 7;
        }
        else if(contName=="North America"){
            return 8;
        }
        else{
            return 0;
        }
    }
    }
}