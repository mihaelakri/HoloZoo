using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


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
        selectedLanguage = PlayerPrefs.GetString("lang","eng"); 
    }

 public void CallRegister(){
    StartCoroutine(Register());
 }

 IEnumerator Register(){

   WWWForm form = new WWWForm();
   form.AddField("username", usernameField.text);
   form.AddField("password", passwordField.text);
   form.AddField("flag", "2");

    if(usernameField.text.Length < 6){
      toast.text = errorMessages[selectedLanguage]["-1"];
    } else if (passwordField.text.Length < 8){
      toast.text = errorMessages[selectedLanguage]["0"];
    } else if (retypepasswordField.text!=passwordField.text){
      toast.text = errorMessages[selectedLanguage]["1"];
    } else{
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
         else if(www.downloadHandler.text != "0"){
            Debug.Log("User created successfully!");
            toast.text = errorMessages[selectedLanguage]["success"];
            Setint(Convert.ToInt16(www.downloadHandler.text));

            if(PlayerPrefs.GetString("device")=="mobile") {
               SceneManager.LoadScene("Home"); 
            } else {
               SceneManager.LoadScene("HologramTablet"); 
            }
           
         }else{
            Debug.Log("User creation failed. Error#" + www.downloadHandler.text);
            toast.text = errorMessages[selectedLanguage]["400"];
         }
      }
    }
 }

     public void Setint(int Value)
    {
        PlayerPrefs.SetInt("ID", Value);
    }

}
