using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class FillLearnAnimals : MonoBehaviour
{
    public int id;
    public GameObject list_element;
    public Animals animals;
    public Font liberationFont;
    public Font jostFont;

    Text[] yourTexts;
    Text[] textComponents;
    Button[] buttons;
    
    [Serializable]
    public class Animal{
        public int id_animal;
        public String name;
    }
      
    [Serializable]
    public class Animals{
        public Animal[] animal;

    }
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("ID")){
           StartCoroutine(FillAnimals());
        }
    }

    IEnumerator FillAnimals(){

        WWWForm form = new WWWForm();
        form.AddField("learn_list", "learn");

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/HoloZoo/animal_view.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    Debug.Log(www.downloadHandler.text);
                    animals = JsonUtility.FromJson<Animals>(www.downloadHandler.text);
                    var x = 0;
                    var y = 234;
                    foreach (var a in animals.animal)
                    {
                       GameObject newobj = Instantiate<GameObject>(list_element);
                       newobj.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
                       newobj.transform.localPosition = new Vector3(0, y, 0);
                       y -= 85;
                       Text newText = newobj.GetComponentInChildren<Text>(); 
                       newText.text = a.name;
                       newobj.tag = a.id_animal.ToString();
                    }
                    // font size
                    yourTexts = FindObjectsOfType<Text>();
                    foreach (Text text in yourTexts)
                    {
                        text.fontSize = PlayerPrefs.GetInt("font_size");
                    }
                    if(PlayerPrefs.GetInt("contrast")==1){
                        buttons = FindObjectsOfType<Button>();
                        foreach (Button button in buttons)
                        {
                            if (button.name != "Back"){
                                Image imageComponent = button.gameObject.GetComponentInChildren<Image>();
                                if (imageComponent != null)
                                {
                                    imageComponent.color = Color.black;
                                }
                            }
                        }
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
        }
    }
}
