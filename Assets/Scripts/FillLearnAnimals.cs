using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FillLearnAnimals : MonoBehaviour
{
    public GameObject list_element;
    public Animals animals;

    [Serializable]
    public class Animal
    {
        public int id_animal;
        public string name;
    }

    [Serializable]
    public class Animals
    {
        public Animal[] animal;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("ID"))
        {
            StartCoroutine(FillAnimals());
        }
    }

    IEnumerator FillAnimals()
    {
        WWWForm form = new();
        form.AddField("learn_list", "learn");

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "animal_view.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("FillLearnAnimals error: " + www.error);
            }
            else
            {
                Debug.Log("FillLearnAnimals: " + www.downloadHandler.text);
                animals = JsonUtility.FromJson<Animals>(www.downloadHandler.text);
                foreach (var a in animals.animal)
                {
                    GameObject newobj = Instantiate(list_element);
                    newobj.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform, false);
                    newobj.transform.GetChild(0).tag = "img-dark";
                    Text newText = newobj.GetComponentInChildren<Text>();
                    newText.text = a.name;
                    newobj.name = a.id_animal.ToString();
                }
                GameObject.Find("AccessHelper").GetComponent<ApplyAccessibility>().LoadAndStyle();
            }
        }
    }
}