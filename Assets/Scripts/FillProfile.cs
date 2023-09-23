using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class FillProfile : MonoBehaviour
{
    public String[] jsonData; 
    public int id; 
    [SerializeField] public Profile_info profile_info; 

    public Text usernameTxt;
    public Text lvlTxt;
    public Text expTxt;
    public Slider expSlider;

    public Font liberationFont;
    public Font jostFont;
    Text[] yourTexts;
    Text[] textComponents;

    [Serializable]
    public class Profile_info{
        public String username;
        public int experience;
        public int level;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("ID")){
           id = PlayerPrefs.GetInt("ID");
           StartCoroutine(Profileinfo());
        }
        //font size
        yourTexts = FindObjectsOfType<Text>();
        foreach (Text text in yourTexts)
        {
            text.fontSize = PlayerPrefs.GetInt("font_size");
        }
        //contrast
        if(PlayerPrefs.GetInt("contrast")==1){
            GameObject.Find("confirmbutton").GetComponent<Image>().color = Color.black;
            textComponents = FindObjectsOfType<Text>();
            
            foreach (Text textComponent in textComponents)
            {
                if(textComponent.text != "CONFIRM"){
                    textComponent.color =  Color.black;
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

    IEnumerator Profileinfo(){

        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("flag", "3");

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/HoloZoo/middle_man.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    profile_info = JsonUtility.FromJson<Profile_info>(www.downloadHandler.text);

                    lvlTxt.text = Convert.ToString(profile_info.level);
                    usernameTxt.text = profile_info.username;
                    expTxt.text = profile_info.experience.ToString() +  "/100";
                    expSlider.value = profile_info.experience;
                }
        }
    }
}