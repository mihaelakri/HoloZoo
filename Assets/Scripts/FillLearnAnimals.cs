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

    Text[] yourTexts;
    
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
        /* yourTexts = FindObjectsOfType<Text>();
        foreach (Text text in yourTexts)
        {
            text.fontSize = PlayerPrefs.GetInt("font_size");
        }*/
    }

    IEnumerator FillAnimals(){

        WWWForm form = new WWWForm();
        // form.AddField("learn_list", "learn");

        using (UnityWebRequest www = UnityWebRequest.Get(CommConstants.ServerURL + "animal/names")){

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
                    var y = 263;
                    foreach (var a in animals.animal)
                    {
                       GameObject newobj = Instantiate(list_element, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Content").transform);
                       newobj.transform.localPosition = new Vector3(0, y, 0);
                       y -= 85;
                        Text newText = newobj.GetComponentInChildren<Text>(); 
                        newText.text = a.name;
                        newobj.tag = a.id_animal.ToString();
                        Debug.Log(a.name);
                    }
                }
        }
    }
}
