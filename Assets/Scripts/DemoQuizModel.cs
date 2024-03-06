using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Threading;

public class DemoQuizModel : MonoBehaviour
{

    public Timer timer; 
    public GameObject prefabPanel; 
    public GameObject helpPanel; 
    public GameObject questPanel; 
    public GameObject finishPopUp;
    public int questionsCount;
    public int questionsCounter;
    public LoadRandomModel loadRandomModelInstance;
    public GameObject model;
    public GameObject accessibility;
    public float elapsedTimeDemo;  // time spent in demo

    // skripta za pokrenuti korutinu i unjeti preferencije u bazu za pojedini zadatak
    public ShowAccessibility showAccessibility; 

    // skripta za pokrenuti korutinu i unjeti kontrole (leap, gumbi, slider) u bazu za pojedini zadatak
    public ControlTracker controlTracker; 

    public int x_exp_min; 
    public int x_exp_max; 
    public int y_exp_min; 
    public int y_exp_max; 

    public int id_question;

    // to know when to go to next question
    public bool nextQuestionFlag = false; 

    // Questions for quiz -->for json
    public Questions questions; 

    [Serializable]
    public class Question{
        public int id_quiz_model_question;
        public int id_animal;
        public String question_text;
        public float initial_rotation_speed; 
        public float initial_size;
        public int x_goal_min; 
        public int x_goal_max; 
        public int y_goal_min;
        public int y_goal_max;
    }
      
    [Serializable]
    public class Questions{
        public Question[] question;
    }

    // Start is called before the first frame update
    void Start()
    {
        elapsedTimeDemo = 0f; 
        prefabPanel.SetActive(false);
        accessibility.SetActive(false);
        questPanel.SetActive(false);
        finishPopUp.SetActive(false);
        StartCoroutine(GetQuestionModels());
    }

    IEnumerator GetQuestionModels(){

        WWWForm form = new WWWForm();
        form.AddField("demo", 1);

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"/quiz_model_view.php", form)){

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

                     // set initial size and speed first
                    CommConstants.initial_size = questions.question[questionsCounter].initial_size; 
                    CommConstants.initial_rotation_speed = questions.question[questionsCounter].initial_rotation_speed;

                    // set inital settings on acc pop-up
                    showAccessibility.setStartingAcc(); 

                    ChangeAnimalModel(questions.question[questionsCounter].id_animal);

                    // Instantiate a prefab (HELP box) 
                    helpPanel = Instantiate<GameObject>(prefabPanel);
                    helpPanel.transform.SetParent(GameObject.FindGameObjectWithTag("canvas").transform, false);
                    helpPanel.SetActive(false);

                    RectTransform panelRectTransform = helpPanel.GetComponent<RectTransform>();
                    RectTransform canvasRectTransform = GameObject.FindGameObjectWithTag("canvas").GetComponent<RectTransform>();

                    // Set the size of the panel to fit within the canvas
                    panelRectTransform.sizeDelta = canvasRectTransform.sizeDelta;
                    panelRectTransform.localPosition = Vector3.zero;
                    panelRectTransform.anchorMin = Vector2.zero;
                    panelRectTransform.anchorMax = Vector2.one;
                    panelRectTransform.anchoredPosition = Vector2.zero;

                    //Fill the Task Panel  
                    Text quest = questPanel.GetComponentInChildren<Text>();
                    quest.text = questions.question[questionsCounter].question_text;

                    //Fill the help panel 
                    Text help = helpPanel.GetComponentInChildren<Text>();
                    help.text = questions.question[questionsCounter].question_text;

                    // Expected values for x and y 
                    x_exp_min = questions.question[questionsCounter].x_goal_min;
                    x_exp_max = questions.question[questionsCounter].x_goal_max;
                    y_exp_min = questions.question[questionsCounter].y_goal_min;
                    y_exp_max = questions.question[questionsCounter].y_goal_max;

                }
        }
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log(x_exp_max + "\n " + y_exp_max + " " + CommConstants.control_type + "\n " + Math.Abs(CommConstants.rotationMsg.x) + " " + Math.Abs(CommConstants.rotationMsg.y) + " " + questionsCounter);
        if (nextQuestionFlag && questionsCounter < questionsCount)
            {
                nextQuestion(); 
                nextQuestionFlag = false; 
            }
        if (questionsCounter == 0 && (Math.Abs(CommConstants.rotationMsg.x) >= x_exp_min || Math.Abs(CommConstants.rotationMsg.x) >= 180-x_exp_min)
                && (Math.Abs(CommConstants.rotationMsg.x) <= x_exp_max || Math.Abs(CommConstants.rotationMsg.x) >= 180-x_exp_max)
                && Math.Abs(CommConstants.rotationMsg.y) >= y_exp_min
                && Math.Abs(CommConstants.rotationMsg.y) <= y_exp_max)
            {
                timer.StopTimer();
                elapsedTimeDemo += timer.GetElapsedTime();
                id_question = questions.question[questionsCounter].id_quiz_model_question;

                questionsCounter++;
                nextQuestionFlag = true;

                // Call SaveControls to start the process
                controlTracker.SaveControls(ControlsSaved);  
          
            }
         else if (questionsCounter == 1 && PlayerPrefs.GetInt("backgorund_white")==1)
            {
                timer.StopTimer();
                elapsedTimeDemo += timer.GetElapsedTime();
                id_question = questions.question[questionsCounter].id_quiz_model_question;

                // Call SaveControls to start the process
                controlTracker.SaveControls(ControlsSaved);

                questionsCounter++;
                nextQuestionFlag = true;
            }
        else if (questionsCounter == 2 && CommConstants.initial_rotation_speed >= 1f && Math.Abs(CommConstants.rotationMsg.x) >= x_exp_min
                && Math.Abs(CommConstants.rotationMsg.x) <= x_exp_max
                && Math.Abs(CommConstants.rotationMsg.y) >= y_exp_min
                && Math.Abs(CommConstants.rotationMsg.y) <= y_exp_max){
                
                timer.StopTimer();
                elapsedTimeDemo += timer.GetElapsedTime();
                id_question = questions.question[questionsCounter].id_quiz_model_question;

                // Call SaveControls to start the process
                controlTracker.SaveControls(ControlsSaved);

                questionsCounter++;
                nextQuestionFlag = true;
        }
        else if(questionsCounter == 3 && CommConstants.control_type == 3 && (Math.Abs(CommConstants.rotationMsg.x) >= x_exp_min || Math.Abs(CommConstants.rotationMsg.x) >= 180-x_exp_min)
                && (Math.Abs(CommConstants.rotationMsg.x) <= x_exp_max || Math.Abs(CommConstants.rotationMsg.x) >= 180-x_exp_max)
                && Math.Abs(CommConstants.rotationMsg.y) >= y_exp_min
                && Math.Abs(CommConstants.rotationMsg.y) <= y_exp_max){

                timer.StopTimer();
                elapsedTimeDemo += timer.GetElapsedTime();
                id_question = questions.question[questionsCounter].id_quiz_model_question;

                // Call SaveControls to start the process
                controlTracker.SaveControls(ControlsSaved);

                questionsCounter++;
                nextQuestionFlag = true;    

        }else if(questionsCounter == 3 && CommConstants.control_type == 3 && (Math.Abs(CommConstants.rotationMsg.x) >= x_exp_min || Math.Abs(CommConstants.rotationMsg.x) >= 180-x_exp_min)
                && (Math.Abs(CommConstants.rotationMsg.x) <= x_exp_max || Math.Abs(CommConstants.rotationMsg.x) >= 180-x_exp_max)
                && Math.Abs(CommConstants.rotationMsg.y) >= y_exp_min
                && Math.Abs(CommConstants.rotationMsg.y) <= y_exp_max)
        {
                timer.StopTimer();
                elapsedTimeDemo += timer.GetElapsedTime();
                id_question = questions.question[questionsCounter].id_quiz_model_question;

                // Call SaveControls to start the process
                controlTracker.SaveControls(ControlsSaved);

                questionsCounter++;
                nextQuestionFlag = true; 
        }else if(questionsCounter > questionsCount){
            finishPopUp.SetActive(true);
        }
    }

    
    void ChangeAnimalModel(int id) {
        loadRandomModelInstance.LoadAnimal(id); 
    }

    void DestroyModel(){
        loadRandomModelInstance.DestroyInstantiatedObject(); 
    }

    public void ShowPrefab()
    {
        helpPanel.SetActive(true); // Set the prefab's active state to false
    }

    public void startQuizDemo(){
        questPanel.SetActive(true);
    }

    public void TimeCounter(){
        timer.StartTimer(); 
    }

    public void start_rotation(){
        // flag za rotaciju za leap motion
         CommConstants.start_quiz_flag = 1;
    }

    public void RestartQuizDemo(){
        questionsCounter = 0;
        questPanel.SetActive(true);
    }

    // Da korutine idu jedna iza druge
    void ControlsSaved(int controlId)
    {
        showAccessibility.saveAccessibilityPrefs(AccessibilitySaved);
    }

    void AccessibilitySaved(int accessibilityId)
    {
        StartCoroutine(AddToDB(id_question, elapsedTimeDemo));
    }

    public void nextQuestion(){

         x_exp_min = questions.question[questionsCounter].x_goal_min;
         x_exp_max = questions.question[questionsCounter].x_goal_max;
         y_exp_min = questions.question[questionsCounter].y_goal_min;
         y_exp_max = questions.question[questionsCounter].y_goal_max;

         // set inital size and speed 
         CommConstants.initial_size = questions.question[questionsCounter].initial_size; 
         CommConstants.initial_rotation_speed = questions.question[questionsCounter].initial_rotation_speed;
        
        // set inital settings on acc pop-up
         showAccessibility.setStartingAcc(); 

         Text quest = questPanel.GetComponentInChildren<Text>();
         quest.text = questions.question[questionsCounter].question_text;

         Text help = helpPanel.GetComponentInChildren<Text>();
         help.text = questions.question[questionsCounter].question_text;

         DestroyModel();
         ChangeAnimalModel(questions.question[questionsCounter].id_animal);
         

         questPanel.SetActive(true);

         // flag za rotaciju za leap motion
         CommConstants.start_quiz_flag = 0;
    }


    // Unos rezultata pojedinog zadataka u bazu 
    IEnumerator AddToDB(int id_question, float time){

        WWWForm form = new WWWForm();
        form.AddField("id_quiz_model", PlayerPrefs.GetInt("quizId"));
        form.AddField("id_quiz_model_question", id_question);
        form.AddField("id_accessibility", PlayerPrefs.GetInt("accessibilityId").ToString());
        form.AddField("id_control", PlayerPrefs.GetInt("controlId").ToString());
        form.AddField("rotate_time", time.ToString());
        

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"/quiz_model_view.php", form)){

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
                    }else{
                        Debug.Log("Success! id result: " + www.downloadHandler.text);
                    }
                   
                }
        }
    }
}
