using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class Login : MonoBehaviour
{
 public InputField usernameField;
 public InputField passwordField;
 public Text toast; 

 public Button submitButton;

 public void CallLogin(){
   if (!Permission.HasUserAuthorizedPermission("android.permission.INTERNET"))
                            Permission.RequestUserPermission("android.permission.INTERNET");
    StartCoroutine(Log_in());
 }

 IEnumerator Log_in(){

    WWWForm form = new WWWForm();
    form.AddField("username", usernameField.text);
    form.AddField("password", passwordField.text);
    form.AddField("flag", "1");

    
    //WWW www = new WWW("http://localhost/HoloZoo/middle_man.php", form);
    WWW www = new WWW(CommConstants.ServerURL+"login", form);
    yield return www;

    Debug.Log(www.text);
    if(www.text == "400"){
       Debug.Log("Bad Request");
    }
    else if(www.text == "404"){
       Debug.Log("Flag not found");
    }
    else if(www.text == "-1"){
        Debug.Log("User login failed. Error#" + www.text);
        toast.text = "Wrong password.";  
    }
    else if(www.text != "0"){ //POPRAVITE DEBUG  
         Debug.Log("Login successfull!");
         int id = Convert.ToInt16(www.text);
         Setint(id);
         CreateRoom createRoom = submitButton.GetComponent<CreateRoom>();
         createRoom.createOrJoinRoom();
         
    }else{
       Debug.Log("User login failed. Error#" + www.text);
       toast.text = "Username invalid.";
    }
    www.Dispose();
    }

   public void Setint(int Value)
    {
        PlayerPrefs.SetInt("ID", Value);
        CommConstants.IdUser = Value.ToString();
    }
 }

