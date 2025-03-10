using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
   public InputField usernameField;
   public InputField passwordField;
   public Text toast;

   public void CallLogin()
   {
      Log_in();
   }

   private void Log_in()
   {
      // Check if GameData is initialized 
      if (GameData.Instance == null)
      {
         Debug.LogError("GameData.Instance is null! Make sure GameData is initialized.");
         return;
      }

      if (usernameField == null || passwordField == null)
      {
         Debug.LogError("Username or Password field is not assigned in the Inspector.");
         return;
      }

      var (user, response) = GameData.Instance.CheckUserCredentials(usernameField.text, passwordField.text);
      var translation = GameData.Instance.translations;

      if (response == GameData.CredentialResponse.WrongUsername)
      {
         Debug.Log($"{nameof(Login)} - Wrong Username");
         toast.text = translation.messages.msg_username_wrong;
         return;
      }

      if (response == GameData.CredentialResponse.WrongPassword)
      {
         Debug.Log($"{nameof(Login)} - Wrong Password");
         toast.text = translation.messages.msg_password_wrong;
         return;
      }

      PlayerPrefs.SetInt("ID", Convert.ToInt16(user.id));

      string sceneToLoad = PlayerPrefs.GetString("device") == "mobile" ? "Home" : "HologramTablet";
      SceneManager.LoadScene(sceneToLoad);
   }
}