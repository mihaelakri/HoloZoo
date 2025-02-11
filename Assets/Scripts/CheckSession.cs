using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CheckSession : MonoBehaviour
{
    public int id; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Session());
    }

   IEnumerator Session(){

        if(PlayerPrefs.HasKey("ID")){

            id = Getint("ID"); 

            WWWForm form = new WWWForm();
            form.AddField("id", id);
            form.AddField("flag", "5");

            using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"middle_man.php", form)){

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log(www.downloadHandler.text);
                    }
            }
        }
      
    }

    public int Getint(string KeyName)
        {
            return PlayerPrefs.GetInt(KeyName);
        }
}

