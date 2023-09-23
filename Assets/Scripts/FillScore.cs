using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FillScore : MonoBehaviour
{
    public Text scoreText;
    public Text message;
    public Sprite happyFace;
    public Sprite sadFace;
    public Image face;
    public Font liberationFont;
    public Font jostFont;

    Text[] textComponents;
    
    void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("Score") + "/" + PlayerPrefs.GetInt("QuestionCount");  
        if((float)PlayerPrefs.GetInt("Score")/(float)PlayerPrefs.GetInt("QuestionCount")<0.5f){
            message.text = "Try again!";
            face.GetComponent<Image>().sprite = sadFace;
        }else{
            message.text = "Bravo!";
            face.GetComponent<Image>().sprite = happyFace;
        }
        StartCoroutine(SetExperience()); 

        if(PlayerPrefs.GetInt("contrast")==1){
            GameObject.Find("Bar").GetComponent<Image>().color = Color.black;
            GameObject.Find("btn").GetComponent<Image>().color = Color.black;
            GameObject.Find("btn2").GetComponent<Image>().color = Color.black;
        }
        //dyslexia
        textComponents = FindObjectsOfType<Text>();
        if(PlayerPrefs.GetInt("dyslexia")==1){
            foreach (Text textComponent in textComponents)
            {
                textComponent.font = liberationFont;
            }
        }else{
            foreach (Text textComponent in textComponents)
            {
                textComponent.font = jostFont;
            }
        }
    }

    IEnumerator SetExperience(){

        WWWForm form = new WWWForm();
        form.AddField("points", PlayerPrefs.GetInt("Score"));


        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"questions/exp", form)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    Debug.Log(www.downloadHandler.text);
                    if(www.downloadHandler.text == "400"){
                        Debug.Log("Bad Request");
                    }
                    else if(www.downloadHandler.text == "0"){
                        Debug.Log("Failed update of experience");
                    }
                    else if(www.downloadHandler.text == "1"){ //POPRAVITE DEBUG  
                        Debug.Log("Experience updated");  
                    }
                }
        }
    }
    
}
