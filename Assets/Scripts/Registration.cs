using System;
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

   private string selectedLanguage;

   private Dictionary<string, Dictionary<string, string>> errorMessages = new Dictionary<string, Dictionary<string, string>>()
   {
      { "en", new Dictionary<string, string>
         {
               { "400", "Check internet connection." },
               { "-1", "Username too short." },
               { "0", "Password too short." },
               { "1", "Passowrds do not match." },
               { "success", "User created successfuly!" }
         }
      },
      { "fr", new Dictionary<string, string>
         {
               { "400", "Vérifiez la connexion Internet." },
               { "-1", "Nom d'utilisateur trop court." },
               { "0", "Mot de passe trop court." },
               { "1", "Les mots de passe ne correspondent pas." },
               { "success", "Utilisateur créé avec succès !" }
         }
      },
      { "hr", new Dictionary<string, string>
         {
               { "400", "Provjerite internetsku vezu." },
               { "-1", "Korisničko ime prekratko." },
               { "0", "Lozinka prekratka." },
               { "1", "Lozinke se ne podudaraju." },
               { "success", "Registracija uspješna!" }
         }
      },
      { "es", new Dictionary<string, string>
         {
               { "400", "Verificar conexión a Internet." },
               { "-1", "Nombre de usuario demasiado corto." },
               { "0", "Contraseña demasiado corta." },
               { "1", "Las contraseñas no coinciden." },
               { "success", "¡Usuario creado exitosamente!" }
         }
      },
      { "hu", new Dictionary<string, string>
         {
               { "400", "Ellenőrizze az internetkapcsolatot." },
               { "-1", "A felhasználónév túl rövid." },
               { "0", "A jelszó túl rövid." },
               { "1", "A jelszavak nem egyeznek." },
               { "success", "Felhasználó sikeresen létrehozva!" }
         }
      }
   };

   void Start()
   {
      selectedLanguage = PlayerPrefs.GetString("lang", "en");
   }

   public void CallRegister()
   {
      StartCoroutine(Register());
   }

   IEnumerator Register()
   {
      if (usernameField.text.Length < 6)
      {
         toast.text = errorMessages[selectedLanguage]["-1"];
         yield break;
      }
      else if (passwordField.text.Length < 8)
      {
         toast.text = errorMessages[selectedLanguage]["0"];
         yield break;
      }
      else if (retypepasswordField.text != passwordField.text)
      {
         toast.text = errorMessages[selectedLanguage]["1"];
         yield break;
      }

      var user = GameData.Instance.CreateUser(usernameField.text, passwordField.text);
      toast.text = errorMessages[selectedLanguage]["success"];
      
      PlayerPrefs.SetInt("ID", Convert.ToInt16(user.id));

      if (PlayerPrefs.GetString("device") == "mobile")
         SceneManager.LoadScene("Home");
      else
         SceneManager.LoadScene("HologramTablet");
   }
}