using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HologramQuizIntro : MonoBehaviour
{
    public Timer timer; 
    public DemoQuizModel demoQuizModel; 
    public float elapsedTimeIntro; 
   

    // Start is called before the first frame update
    void Start()
    {
        timer.StartTimer();
    }

    public void stopTimerIntro(){
        timer.StopTimer();
        elapsedTimeIntro = timer.GetElapsedTime();
        StartCoroutine(insertQuiz(elapsedTimeIntro)); 
    }

    public void startDemo(){
        demoQuizModel.startQuizDemo();
    }

    IEnumerator insertQuiz(float elapsedTimeIntro){

        WWWForm form = new WWWForm();
        form.AddField("time_intro", elapsedTimeIntro.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"quiz_model_view.php", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
            {
                if(www.downloadHandler.text == "0")
                {
                    Debug.Log("Error! The quiz is not added to database!");
                }
                else
                {
                    int quizId = Convert.ToInt16(www.downloadHandler.text);
                    PlayerPrefs.SetInt("quizId", quizId);
                    Debug.Log("Quiz added to database with ID: " + quizId);
                }

            }
        }
    }
}
