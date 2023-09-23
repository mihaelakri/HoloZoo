using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    public int id; 
    [SerializeField] public Profile_info profile_info; 

    [Serializable]
    public class Profile_info{
        public String username;
        public int experience;
        public int level;
    }
    
    // Start is called before the first frame update
    public void createOrJoinRoom()
    {
        if(PlayerPrefs.HasKey("ID")){
           id = PlayerPrefs.GetInt("ID");
           StartCoroutine(GetUsername());
        }
        
        if(PlayerPrefs.GetString("device")=="mobile"){
            PhotonNetwork.CreateRoom(PlayerPrefs.GetString("username"));
            PhotonNetwork.LoadLevel("Home");
        }else{
            PhotonNetwork.JoinRoom(PlayerPrefs.GetString("username"));
            PhotonNetwork.LoadLevel("HologramTablet");
        }
        
    }

    IEnumerator GetUsername(){

        WWWForm form = new WWWForm();
        // form.AddField("id", id);
        // form.AddField("flag", "3");

        using (UnityWebRequest www = UnityWebRequest.Get(CommConstants.ServerURL+"session/username")){

            yield return www.SendWebRequest();

            Debug.Log(www.downloadHandler.text);
            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                    Debug.Log("eroric");
                }
            else
                {
                    profile_info = JsonUtility.FromJson<Profile_info>(www.downloadHandler.text);
                    Debug.Log(profile_info.username);
                    PlayerPrefs.SetString("username", profile_info.username);
                }
        }
    }

    public override void OnCreatedRoom()
    {
        // Room created successfully
        Debug.Log("Room created.");
    }
}
