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
    
    // Start is called before the first frame update
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
    }

    IEnumerator SetExperience(){

        WWWForm form = new WWWForm();
        form.AddField("points", PlayerPrefs.GetInt("Score"));


        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/HoloZoo/quiz_view.php", form)){

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
