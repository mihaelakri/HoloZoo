using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class FillAnimal : MonoBehaviour
{
    public Text nameText;
    public Text endangermentStatusText;
    public Text regionText;
    public Text habitatsText;
    public Text weightText;
    public Text dietText;
    public Text populationText;
    public GameObject animal_photo;

    public Font liberationFont;
    public Font jostFont;
    Button[] buttons;
    GameObject[] objectsWithTag;
    Text[] textComponents;

    [Serializable]
    public class Animal{
        public String area;
        public String name;
        public String url_slika;
        public String endangerment_status;
        public String habitat;
        public String weight;
        public String diet;
        public String population;
    }

    public Animal animal;

    void Start()
    {
        StartCoroutine(FillAnimalInfo());
        //contrast
        if(PlayerPrefs.GetInt("contrast")==1){
            buttons = FindObjectsOfType<Button>();
            foreach (Button button in buttons)
            {
                Debug.Log(buttons.Length);
                if (button.name != "Back" && button.name != "AnimalIcon"){
                    Image imageComponent = button.gameObject.GetComponentInChildren<Image>();
                    imageComponent.color = Color.black;
                    Text textComponent = button.GetComponentInChildren<Text>();
                    if(textComponent!=null){
                        textComponent.color = Color.white;
                    }
                }
            }
            GameObject.Find("ES-dropdown").GetComponent<Image>().color = Color.gray;
            GameObject.Find("Region-dropdown").GetComponent<Image>().color = Color.gray;
            GameObject.Find("Habitats-dropdown").GetComponent<Image>().color = Color.gray;
            GameObject.Find("Weight-dropdown").GetComponent<Image>().color = Color.gray;
            GameObject.Find("Diet-dropdown").GetComponent<Image>().color = Color.gray;
            GameObject.Find("Population-dropdown").GetComponent<Image>().color = Color.gray;

        }
    	//dyslexia
        textComponents = FindObjectsOfType<Text>();
        if(PlayerPrefs.GetInt("dyslexia")==1){
            foreach (Text textComponent in textComponents)
            {
                textComponent.font = liberationFont;
            }
        }else{
            foreach (Text textComponent in textComponents)
            {
                textComponent.font = jostFont;
            }
        }
    }

    IEnumerator FillAnimalInfo(){

        WWWForm form = new WWWForm();
        form.AddField("id_animal", PlayerPrefs.GetString("id_animal"));
        string animal_id =  PlayerPrefs.GetString("id_animal");

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/HoloZoo/animal_view.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    animal = JsonUtility.FromJson<Animal>(www.downloadHandler.text);
                    UnityWebRequest request = UnityWebRequestTexture.GetTexture("http://localhost/HoloZoo/" + animal.url_slika);
                    Debug.Log("http://localhost/HoloZoo/" + animal.url_slika);
                    yield return request.SendWebRequest();

                    nameText.text = animal.name;
                    endangermentStatusText.text = animal.endangerment_status;
                    regionText.text = animal.area;
                    habitatsText.text = animal.habitat;
                    weightText.text = animal.weight;
                    dietText.text = animal.diet;
                    populationText.text = animal.population;

                    if (request.isNetworkError || request.isHttpError)
                    {
                        Debug.LogError(request.error);
                    }else
                    {
                        Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                        Image img = animal_photo.AddComponent<Image>();
                        img.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2());
                        
                    }
                }
        }
    }

}
