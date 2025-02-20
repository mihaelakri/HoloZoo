using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Quiz : MonoBehaviour
{ 
    public Text questionText;
    public Text answerAText;
    public Text answerBText;
    public Text answerCText;
    public Questions questions; 
    public int questionsCount;
    public int questionsCounter;

    public Sprite correct;
    public Sprite wrong;
    public Sprite wrongy;
    public Sprite neutral;

    public Button btnAnswerA; 
    public Button btnAnswerB; 
    public Button btnAnswerC;
    public Button nextButton;

    Text[] yourTexts;

    public int correctAnswerCount; 

    [Serializable]
    public class Question{
        public int id_question;
        public int id_animal;
        public String question_text;
        public String correct_answer;
        public String answer_one;
        public String answer_two;
        public String answer_three;
    }
      
    [Serializable]
    public class Questions{
        public Question[] question;
    }
    // Start is called before the first frame update
    void Start()
    {   
        yourTexts = FindObjectsOfType<Text>();
        foreach (Text text in yourTexts)
        {
            text.fontSize = PlayerPrefs.GetInt("font_size");
            Debug.Log(text.text);
        }
        
        StartCoroutine(FillQuestion());
    }

    IEnumerator FillQuestion(){

        // WWWForm form = new WWWForm();
        // form.AddField("difficulty", PlayerPrefs.GetInt("diff"));
        int difficulty = PlayerPrefs.GetInt("diff");

        using (UnityWebRequest www = UnityWebRequest.Get(CommConstants.ServerURL+"questions/random/"+difficulty)){

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            else
                {
                    if(www.downloadHandler.text == "404"){
                        Debug.Log("Invalid input.");
                    }
                    questions = JsonUtility.FromJson<Questions>(www.downloadHandler.text);

                    questionsCount = questions.question.Length; 
                    questionsCounter = 0;
                    // Fill data
                    questionText.text = questions.question[questionsCounter].question_text;
                    answerAText.text = questions.question[questionsCounter].answer_one;
                    answerBText.text = questions.question[questionsCounter].answer_two;
                    answerCText.text = questions.question[questionsCounter].answer_three;

                
                    if(PlayerPrefs.GetInt("textToSpeech")==1){
                        GameObject.Find("TTSHelper").GetComponent<TtsGlobal>().readQuiz(questions.question[questionsCounter].question_text, questions.question[questionsCounter].answer_one, questions.question[questionsCounter].answer_two, questions.question[questionsCounter].answer_three);
                    }
                   
                }
        }
    }

    public void CheckAnswer(){
        Debug.Log(EventSystem.current.currentSelectedGameObject.tag);
        String id = EventSystem.current.currentSelectedGameObject.tag; 
        if(PlayerPrefs.GetInt("textToSpeech")==1){
            GameObject.Find("TTSHelper").GetComponent<TtsGlobal>().doSpeak(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        }
        //if correct make it green
        if(id == questions.question[questionsCounter].correct_answer){
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = correct;
            String chosenTag = EventSystem.current.currentSelectedGameObject.tag;
            //color other two
            if(chosenTag == btnAnswerA.tag){
                btnAnswerB.GetComponent<Image>().sprite = wrongy;
                btnAnswerC.GetComponent<Image>().sprite = wrongy;
            }else if(chosenTag == btnAnswerB.tag){
                btnAnswerA.GetComponent<Image>().sprite = wrongy;
                btnAnswerC.GetComponent<Image>().sprite = wrongy;
            }else{
                btnAnswerB.GetComponent<Image>().sprite = wrongy;
                btnAnswerA.GetComponent<Image>().sprite = wrongy;
            }
            correctAnswerCount++;
        }else{
            //if incorrect color red
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = wrong;
            //if A correct but, not chosen
            if(btnAnswerA.tag == questions.question[questionsCounter].correct_answer){
                btnAnswerA.GetComponent<Image>().sprite = correct;
                //check which one is chosen to color red
                if(id==btnAnswerB.tag){
                    btnAnswerB.GetComponent<Image>().sprite = wrong;
                    btnAnswerC.GetComponent<Image>().sprite = wrongy;
                }else{
                    btnAnswerC.GetComponent<Image>().sprite = wrong;
                    btnAnswerB.GetComponent<Image>().sprite = wrongy;
                }
            }else if(btnAnswerB.tag == questions.question[questionsCounter].correct_answer){
                //if B correct but, not chosen, color wrong one red and other light green
                btnAnswerB.GetComponent<Image>().sprite = correct;
                //check which one is chosen to color red
                if(id==btnAnswerC.tag){
                    btnAnswerC.GetComponent<Image>().sprite = wrong;
                    btnAnswerA.GetComponent<Image>().sprite = wrongy;
                }else{
                    btnAnswerA.GetComponent<Image>().sprite = wrong;
                    btnAnswerC.GetComponent<Image>().sprite = wrongy;
                }
            }else{
                //if C correct but, not chosen, color wrong one red and other light green
                btnAnswerC.GetComponent<Image>().sprite = correct;
                //check which one is chosen to color red
                if(id==btnAnswerB.tag){
                    btnAnswerB.GetComponent<Image>().sprite = wrong;
                    btnAnswerA.GetComponent<Image>().sprite = wrongy;
                }else{
                    btnAnswerA.GetComponent<Image>().sprite = wrong;
                    btnAnswerB.GetComponent<Image>().sprite = wrongy;
                }
            }
        }
        btnAnswerA.enabled = false;
        btnAnswerB.enabled = false;
        btnAnswerC.enabled = false;
        questionsCounter++;
        nextButton.gameObject.SetActive(true);
        nextButton.transform.LeanMoveLocal(new Vector2(123,-225),1).setEaseOutQuart();
        if(questionsCounter<questionsCount){
            nextButton.transform.GetChild(0).GetComponent<Text>().text = "Next question";
        }else{
            nextButton.transform.GetChild(0).GetComponent<Text>().text = "See results";
        }
    }

    public void nextQuestion(){
        nextButton.gameObject.SetActive(false);
        nextButton.transform.LeanMoveLocal(new Vector2(-27,-446),1).setEaseOutQuart();
        if(nextButton.transform.GetChild(0).GetComponent<Text>().text == "See results"){
            PlayerPrefs.SetInt("Score",correctAnswerCount);
            PlayerPrefs.SetInt("QuestionCount",questionsCount);
            SceneManager.LoadScene("Score");  
        }
        btnAnswerA.GetComponent<Image>().sprite = neutral;
        btnAnswerB.GetComponent<Image>().sprite = neutral;
        btnAnswerC.GetComponent<Image>().sprite = neutral;
        questionText.text = questions.question[questionsCounter].question_text;
        answerAText.text = questions.question[questionsCounter].answer_one;
        answerBText.text = questions.question[questionsCounter].answer_two;
        answerCText.text = questions.question[questionsCounter].answer_three;
        btnAnswerA.enabled = true;
        btnAnswerB.enabled = true;
        btnAnswerC.enabled = true;

        if(PlayerPrefs.GetInt("textToSpeech")==1){
            GameObject.Find("TTSHelper").GetComponent<TtsGlobal>().readQuiz(questions.question[questionsCounter].question_text, questions.question[questionsCounter].answer_one, questions.question[questionsCounter].answer_two, questions.question[questionsCounter].answer_three);
        }

    }
    
    public int Getint(string KeyName)
    {
        return PlayerPrefs.GetInt(KeyName);
    }

}
