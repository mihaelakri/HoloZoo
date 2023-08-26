using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Photon.Pun;

public class CreateRoom : MonoBehaviour
{
      public String[] jsonData; 
    public int id; 
    [SerializeField] public Profile_info profile_info; 

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
           StartCoroutine(GetUsername());
        }
        PhotonNetwork.CreateRoom(PlayerPrefs.GetString("username"));
    }

    IEnumerator GetUsername(){

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
                    PlayerPrefs.SetString("username", profile_info.username);
                }
        }
    }
}
