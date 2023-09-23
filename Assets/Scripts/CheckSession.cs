using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSession : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Session());
    }

   IEnumerator Session(){

      WWW www = new WWW(CommConstants.ServerURL+"session/id");
      yield return www;

      Debug.Log(www.text);
      www.Dispose();

      if(PlayerPrefs.HasKey("ID")){
            Debug.Log(Getint("ID"));
        }
    }

    public int Getint(string KeyName)
        {
            return PlayerPrefs.GetInt(KeyName);
        }
}

