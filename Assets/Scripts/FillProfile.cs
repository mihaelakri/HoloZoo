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

    Text[] yourTexts;

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
           id = Getint("ID");
           StartCoroutine(Profileinfo());
        }
         yourTexts = FindObjectsOfType<Text>();
        foreach (Text text in yourTexts)
        {
            text.fontSize = PlayerPrefs.GetInt("font_size");
            Debug.Log(text.text);
        }
    }

    IEnumerator Profileinfo(){

        // WWWForm form = new WWWForm();
        // form.AddField("id", id);
        // form.AddField("flag", "3");

        using (UnityWebRequest www = UnityWebRequest.Get(CommConstants.ServerURL+"profile/"+id)){

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
    


    public int Getint(string KeyName)
        {
            return PlayerPrefs.GetInt(KeyName);
        }
}
