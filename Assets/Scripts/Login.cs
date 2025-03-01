using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
   public InputField usernameField;
   public InputField passwordField;
   public Text toast;

   public Button submitButton;

   private string selectedLanguage;

   private Dictionary<string, Dictionary<string, string>> errorMessages = new Dictionary<string, Dictionary<string, string>>()
   {
      { "en", new Dictionary<string, string>
         {
               { "400", "Check internet connection." },
               { "-1", "Wrong password." },
               { "0", "Username invalid." },
               { "success", "Login successful!" }
         }
      },
      { "fr", new Dictionary<string, string>
         {
               { "400", "Vérifiez la connexion Internet." },
               { "-1", "Mot de passe incorrect." },
               { "0", "Nom d'utilisateur non valide." },
               { "success", "Connexion réussie !" }
         }
      },
      { "hr", new Dictionary<string, string>
         {
               { "400", "Provjerite internetsku vezu." },
               { "-1", "Pogrešna lozinka." },
               { "0", "Neispravno korisničko ime." },
               { "success", "Prijava uspješna!" }
         }
      },
      { "es", new Dictionary<string, string>
         {
               { "400", "Verificar conexión a Internet." },
               { "-1", "Contraseña incorrecta." },
               { "0", "Nombre de usuario no válido." },
               { "success", "¡Inicio de sesión exitoso!" }
         }
      },
      { "hu", new Dictionary<string, string>
         {
               { "400", "Ellenőrizze az internetkapcsolatot." },
               { "-1", "Rossz jelszó." },
               { "0", "Érvénytelen felhasználónév." },
               { "success", "Sikeres bejelentkezés!" }
         }
      }
   };

   void Start()
   {
      selectedLanguage = PlayerPrefs.GetString("lang", "en");
   }

   public void CallLogin()
   {
      StartCoroutine(Log_in());
   }

   IEnumerator Log_in()
   {
      var (user, response) = GameData.Instance.CheckUserCredentials(usernameField.text, passwordField.text);

      if (response == GameData.CredentialResponse.WrongUsername)
      {
         Debug.Log($"{nameof(Login)} - Wrong Username");
         toast.text = errorMessages[selectedLanguage]["0"];
         yield break;
      }

      if (response == GameData.CredentialResponse.WrongPassword)
      {
         Debug.Log($"{nameof(Login)} - Wrong Password");
         toast.text = errorMessages[selectedLanguage]["-1"];
         yield break;
      }

      PlayerPrefs.SetInt("ID", Convert.ToInt16(user.id));

      if (PlayerPrefs.GetString("device") == "mobile")
         SceneManager.LoadScene("Home");
      else
         SceneManager.LoadScene("HologramTablet");
   }
}