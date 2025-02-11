using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Android;

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
        selectedLanguage = PlayerPrefs.GetString("lang","eng"); 
    }

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
    

   using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"middle_man.php", form)){

      yield return www.SendWebRequest();

         if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
         if (www.downloadHandler.text == "400" || www.downloadHandler.text == "404")
            {
               Debug.Log("Bad Request or Flag not found");
               toast.text = errorMessages[selectedLanguage]["400"];
            }
            else if (www.downloadHandler.text == "-1")
            {
               Debug.Log("User login failed. Error#" + www.downloadHandler.text);
               toast.text = errorMessages[selectedLanguage]["-1"];
            }
            else if (www.downloadHandler.text != "0") 
            {
               Debug.Log("Response: " + www.downloadHandler.text);
               Debug.Log(errorMessages[selectedLanguage]["success"]);
               int id = Convert.ToInt16(www.downloadHandler.text);
               Setint(id);
                if(PlayerPrefs.GetString("device")=="mobile") {
                  SceneManager.LoadScene("Home"); 
               } else {
                  SceneManager.LoadScene("HologramTablet"); 
               }
            }
            else
            {
               Debug.Log("User login failed. Error#" + www.downloadHandler.text);
               toast.text = errorMessages[selectedLanguage]["0"];
            }
         }
   }

   public void Setint(int Value)
    {
        PlayerPrefs.SetInt("ID", Value);
    }
 }

