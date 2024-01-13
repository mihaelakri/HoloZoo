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
    private string sceneNameToLoad = "Instructions_one";

    private float timeElapsed;

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= delayBeforeLoading){
            SceneManager.LoadScene(sceneNameToLoad);
            if(PlayerPrefs.HasKey("ID")){
                id = PlayerPrefs.GetInt("ID");
                StartCoroutine(setSession());
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
        form.AddField("flag", "5");

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"middle_man.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    Debug.Log(www.downloadHandler.text);
                }
        }
    }

}
