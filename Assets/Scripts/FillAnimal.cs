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
    }

    IEnumerator FillAnimalInfo(){

        WWWForm form = new WWWForm();
        form.AddField("id_animal", PlayerPrefs.GetString("id_animal"));

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
