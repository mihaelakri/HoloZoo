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
      StartCoroutine(Log_in());
   }

   IEnumerator Log_in()
   {
      var (user, response) = GameData.Instance.CheckUserCredentials(usernameField.text, passwordField.text);
      var translation = GameData.Instance.translations;

      if (response == GameData.CredentialResponse.WrongUsername)
      {
         Debug.Log($"{nameof(Login)} - Wrong Username");
         toast.text = translation.messages.msg_username_wrong;
         yield break;
      }

      if (response == GameData.CredentialResponse.WrongPassword)
      {
         Debug.Log($"{nameof(Login)} - Wrong Password");
         toast.text = translation.messages.msg_password_wrong;
         yield break;
      }

      PlayerPrefs.SetInt("ID", Convert.ToInt16(user.id));

      if (PlayerPrefs.GetString("device") == "mobile")
         SceneManager.LoadScene("Home");
      else
         SceneManager.LoadScene("HologramTablet");
   }
}