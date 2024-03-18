using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class QuizModels : MonoBehaviour
{
    public Timer timer;
    public GameObject prefabPanel;
    public GameObject helpPanel;
    public GameObject questPanel;
    public GameObject finishPopUp;
    public int questionsCount;
    public int questionsCounter=0;
    public LoadRandomModel loadRandomModelInstance;
    public GameObject model;
    public GameObject accessibility;
    public ShowAccessibility showAccessibility;
    public float elapsedTime;  // time spent on each task
    public Slider bottomSlider;
    public Slider sideSlider;

    // skripta za pokrenuti korutinu i unjeti kontrole (leap, gumbi, slider) u bazu za pojedini zadatak
    public ControlTracker controlTracker;

    public int x_exp_min;
    public int x_exp_max;
    public int y_exp_min;
    public int y_exp_max;

    public int id_question;

    // to know when to go to next question
    public bool nextQuestionFlag = false;

    // zbog update (pretekne korutinu)
    public bool startQuizFlag = false;

    // Questions for quiz -->for json
    public Questions questions;

    [Serializable]
    public class Question
    {
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
    public class Questions
    {
        public Question[] question;
    }

    // Start is called before the first frame update
    void Start()
    {
        prefabPanel.SetActive(false);
        finishPopUp.SetActive(false);
        accessibility.SetActive(false);
        StartCoroutine(GetQuestionModels());
    }

    IEnumerator GetQuestionModels()
    {

        WWWForm form = new WWWForm();
        form.AddField("models", 1);

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "quiz_model_view.php", form))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.downloadHandler.text == "404")
                {
                    Debug.Log("Invalid input.");
                }

                questions = JsonUtility.FromJson<Questions>(www.downloadHandler.text);

                questionsCount = questions.question.Length;
                questionsCounter = 0;

                // set initial size and speed first
                CommConstants.state.initial_size = questions.question[questionsCounter].initial_size;
                CommConstants.state.initial_rotation_speed = questions.question[questionsCounter].initial_rotation_speed;

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

                startQuizFlag = true;
                
          
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Question count " + questionsCount + " question counter " + questionsCounter);
        //Debug.Log("QQ: " + questionsCounter + " " + questionsCount + " X: " + x_exp_min + " - " + x_exp_max + " Y: " + y_exp_min + " - " + y_exp_max + " IS NOW: " + Math.Abs(CommConstants.rotation.x) + " " + Math.Abs(CommConstants.rotation.y));
        if (nextQuestionFlag && questionsCounter < questionsCount)
        {
            nextQuestion();
            nextQuestionFlag = false;
        }
       

        if (startQuizFlag == true && Math.Abs(CommConstants.rotation.x) >= x_exp_min
                && Math.Abs(CommConstants.rotation.x) <= x_exp_max
                && Math.Abs(CommConstants.rotation.y) >= y_exp_min
                && Math.Abs(CommConstants.rotation.y) <= y_exp_max)
        {
           
            if (questionsCounter >= questionsCount)
            {               
                finishPopUp.SetActive(true);
            }
            else
            {
                timer.StopTimer();
                elapsedTime += timer.GetElapsedTime();
                id_question = questions.question[questionsCounter].id_quiz_model_question;

                //Call SaveControls to start the process
                controlTracker.SaveControls(ControlsSaved);
                questionsCounter++;
                nextQuestionFlag = true;
            }
          
        }
      
    }

    public void start_rotation()
    {
        // flag za rotaciju za leap motion
        CommConstants.state.start_quiz_flag = 1;

        CommConstants.rotation.x = 0;
        CommConstants.rotation.y = 0;


        bottomSlider.interactable = true;
        sideSlider.interactable = true;
    }


    void ChangeAnimalModel(int id)
    {
        loadRandomModelInstance.LoadAnimal(id);
        CommConstants.animalid.animal_id = id.ToString();
    }

    void DestroyModel()
    {
        loadRandomModelInstance.DestroyInstantiatedObject();
    }

    public void ShowPrefab()
    {
        helpPanel.SetActive(true); // Set the prefab's active state to false
    }

    public void TimeCounter()
    {
        timer.StartTimer();
    }

    // Da korutine idu jedna iza druge
    void ControlsSaved(int controlId)
    {
        showAccessibility.saveAccessibilityPrefs(AccessibilitySaved);
    }

    void AccessibilitySaved(int accessibilityId)
    {
        StartCoroutine(AddToDB(id_question, elapsedTime));
    }

    public void nextQuestion()
    {

        x_exp_min = questions.question[questionsCounter].x_goal_min;
        x_exp_max = questions.question[questionsCounter].x_goal_max;
        y_exp_min = questions.question[questionsCounter].y_goal_min;
        y_exp_max = questions.question[questionsCounter].y_goal_max;

        // set inital size and speed 
        CommConstants.state.initial_size = questions.question[questionsCounter].initial_size;
        CommConstants.state.initial_rotation_speed = questions.question[questionsCounter].initial_rotation_speed;

       
        Text quest = questPanel.GetComponentInChildren<Text>();
        quest.text = questions.question[questionsCounter].question_text;

        Text help = helpPanel.GetComponentInChildren<Text>();
        help.text = questions.question[questionsCounter].question_text;

        DestroyModel();
        ChangeAnimalModel(questions.question[questionsCounter].id_animal);

      
        // set inital settings on acc pop-up
        showAccessibility.setStartingAcc();

        questPanel.SetActive(true);
        CommConstants.state.start_quiz_flag = 0;

        bottomSlider.interactable=false;
        sideSlider.interactable=false;

    }


    // Unos rezultata pojedinog zadataka u bazu 
    IEnumerator AddToDB(int id_question, float time)
    {

        WWWForm form = new WWWForm();
        form.AddField("id_quiz_model", PlayerPrefs.GetInt("quizId"));
        form.AddField("id_quiz_model_question", id_question);
        form.AddField("id_accessibility", PlayerPrefs.GetInt("accessibilityId").ToString());
        form.AddField("id_control", PlayerPrefs.GetInt("controlId").ToString());
        form.AddField("rotate_time", time.ToString());


        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL + "quiz_model_view.php", form))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "400")
                {
                    Debug.Log("Bad Request");
                }
                else
                {
                    Debug.Log("Success! id result: " + www.downloadHandler.text);
                }

            }
        }
    }

    public void QuitApp()
    {
      
        // Quit the application
        Application.Quit();
    }
}
