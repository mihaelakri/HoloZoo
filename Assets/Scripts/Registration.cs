using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Registration : MonoBehaviour
{
 
 public InputField usernameField;
 public InputField passwordField;
 public InputField retypepasswordField;
 public Text toast; 

 public Button submitButton;

 public void CallRegister(){
    StartCoroutine(Register());
 }

 IEnumerator Register(){

    WWWForm form = new WWWForm();
    form.AddField("username", usernameField.text);
    form.AddField("password", passwordField.text);
   //  form.AddField("flag", "2");

    if(usernameField.text.Length < 6){
      toast.text = "Username too short";
    } else if (passwordField.text.Length < 8){
      toast.text = "Password too short";
    } else if (retypepasswordField.text!=passwordField.text){
      toast.text = "Passwords do not match";
    } else{
      WWW www = new WWW(CommConstants.ServerURL+"register", form);
      yield return www;

      Debug.Log(www.text);
      if(www.text == "400"){
         Debug.Log("Bad Request");
      }
      else if(www.text == "404"){
         Debug.Log("Flag not found");
      }
      else if(www.text != "0"){
         Debug.Log("User created successfully!");
         Setint(Convert.ToInt16(www.text));
         if(PlayerPrefs.GetString("device")=="mobile"){
            SceneManager.LoadScene("Home");
         }else{
            SceneManager.LoadScene("HologramTablet");
         }  
      }else{
         Debug.Log("User creation failed. Error#" + www.text);
      }
      www.Dispose();
    }
 }

     public void Setint(int Value)
    {
        PlayerPrefs.SetInt("ID", Value);
    }

}
