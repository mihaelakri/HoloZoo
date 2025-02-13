using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

public class LoadingScreen : MonoBehaviour
{
    public int id; 
    [SerializeField]
    private float delayBeforeLoading = 5f;
    [SerializeField]
    private string sceneNameToLoad = "Language";

    private float timeElapsed;
    private bool sessionSet = false;
    private bool sessionCoroutineStarted = false;

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Make web request immediately to avoid delay
        if(PlayerPrefs.HasKey("ID") && !sessionCoroutineStarted){
            sessionCoroutineStarted = true;
            StartCoroutine(setSession());
        }

        if (timeElapsed >= delayBeforeLoading){
            // Make sure session is set before loading scene
             if(PlayerPrefs.HasKey("ID") && sessionSet){
                 id = Getint("ID");
                 if(PlayerPrefs.GetString("device")=="mobile"){
                     SceneManager.LoadScene("Home");
                 }else{
                     SceneManager.LoadScene("HologramTablet");
                 }
             }else{
                 SceneManager.LoadScene(sceneNameToLoad);
             }  
        }
    }

    IEnumerator setSession(){

        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("lang", PlayerPrefs.GetString("lang", "en"));
        form.AddField("flag", "5");

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"middle_man.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("set session error: " + www.error);
                }
            else
                {
                    Debug.Log("set session: " + www.downloadHandler.text);
                    sessionSet = true;
                }
        }
    }

    public int Getint(string KeyName)
        {
            return PlayerPrefs.GetInt(KeyName);
        }
}
