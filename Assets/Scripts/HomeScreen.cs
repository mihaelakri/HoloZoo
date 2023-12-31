using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class HomeScreen : MonoBehaviourPunCallbacks
{
    public int id; 
    [SerializeField] public Profile_info profile_info; 

    [Serializable]
    public class Profile_info{
        public String username;
        public int experience;
        public int level;
    }
    
    // Start is called before the first fram
    void Start()
    {
        if(PlayerPrefs.GetInt("contrast")==1){
            GameObject.Find("Bar").GetComponent<Image>().color = Color.black; 
            GameObject.Find("btn1").GetComponent<Image>().color = Color.black; 
            GameObject.Find("btn2").GetComponent<Image>().color = Color.black; 
        }else{
            GameObject.Find("Bar").GetComponent<Image>().color = Color.white; 
            GameObject.Find("btn1").GetComponent<Image>().color = Color.white; 
            GameObject.Find("btn2").GetComponent<Image>().color = Color.white; 
        }
       if(PlayerPrefs.HasKey("ID")){
           id = PlayerPrefs.GetInt("ID");
           StartCoroutine(GetUsername());
        }
        PhotonNetwork.CreateRoom(PlayerPrefs.GetString("username"));
        if(PlayerPrefs.GetInt("dyslexia")==1){
             
        }
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
                    Debug.Log("eroric");
                }
            else
                {
                    profile_info = JsonUtility.FromJson<Profile_info>(www.downloadHandler.text);
                    PlayerPrefs.SetString("username", profile_info.username);
                }
        }
    }

}
