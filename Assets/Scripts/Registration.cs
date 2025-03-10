using System;
using System.Collections;
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

   public void CallRegister()
   {
     Register();
   }

   private void Register()
   {
      var translation = GameData.Instance.translations;

      if (usernameField.text.Length < 6)
      {
         toast.text = translation.messages.msg_username_short;
         return;
      }
      else if (passwordField.text.Length < 8)
      {
         toast.text = translation.messages.msg_password_short;
         return;
      }
      else if (retypepasswordField.text != passwordField.text)
      {
         toast.text = translation.messages.msg_passwords_mismatch;
         return;
      }

      var user = GameData.Instance.CreateUser(usernameField.text, passwordField.text);
      toast.text = translation.messages.msg_register_success;

      PlayerPrefs.SetInt("ID", Convert.ToInt16(user.id));

      string sceneToLoad = PlayerPrefs.GetString("device") == "mobile" ? "Home" : "HologramTablet";
      SceneManager.LoadScene(sceneToLoad);
   }
}