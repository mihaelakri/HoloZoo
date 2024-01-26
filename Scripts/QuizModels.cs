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
    public int questionsCount;
    public int questionsCounter;
    public LoadRandomModel loadRandomModelInstance;
    public GameObject model;
    public GameObject accessibility;

    public int x_exp; 
    public int y_exp; 

    // Questions for quiz -->for json
    public Questions questions; 

    [Serializable]
    public class Question{
        public int id_quiz_model;
        public String question_text;
        public int control_type;
        public int x_goal; 
        public int y_goal;
    }
      
    [Serializable]
    public class Questions{
        public Question[] question;
    }

    // Start is called before the first frame update
    void Start()
    {
        accessibility.SetActive(false);
        StartCoroutine(GetQuestionModels());
        ChangeAnimalModel();
    }

    IEnumerator GetQuestionModels(){

        WWWForm form = new WWWForm();
        form.AddField("models", 1);

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"/quiz_view.php", form)){

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
                    x_exp = questions.question[questionsCounter].x_goal;
                    y_exp = questions.question[questionsCounter].y_goal;
                    
                    CommConstants.control_type =  questions.question[questionsCounter].control_type; 
                    CheckControlType(); 
                    Debug.Log("Control type: " +CommConstants.control_type);
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(CommConstants.x + " " + CommConstants.y);

        // Check if questions array is not null and questionsCounter is within bounds
        if (questions != null && questionsCounter >= 0 && questionsCounter < questionsCount)
        {
            if (CommConstants.x == x_exp && CommConstants.y == y_exp)
            {
                
                //Debug.Log("x: " + CommConstants.x + " y: " + CommConstants.y);
                //Debug.Log("x-exp: " + x_exp + " y-exp: " + y_exp);

                timer.StopTimer();
                float rotate_time = timer.GetElapsedTime();
                //Debug.Log(rotate_time);

                StartCoroutine(AddToDB(rotate_time, CommConstants.control_type));

                questionsCounter++;

                // Check if questionsCounter is still within bounds after incrementing
                if (questionsCounter < questionsCount)
                {
                    x_exp = questions.question[questionsCounter].x_goal;
                    y_exp = questions.question[questionsCounter].y_goal;

                    CommConstants.control_type = questions.question[questionsCounter].control_type;
                    CheckControlType();

                    Text quest = questPanel.GetComponentInChildren<Text>();
                    quest.text = questions.question[questionsCounter].question_text;

                    Text help = helpPanel.GetComponentInChildren<Text>();
                    help.text = questions.question[questionsCounter].question_text;

                    DestroyModel();
                    ChangeAnimalModel();

                    questPanel.SetActive(true);
                }
                else
                {
                    Debug.Log("No more questions. Quiz completed.");
                    // Optionally, you can handle the completion of the quiz here.
                }
            }
        }
        else
        {
            Debug.LogWarning("Invalid questions or questionsCounter value.");
        }
    }

    void ChangeAnimalModel() {
        loadRandomModelInstance.LoadAnimal(); 
    }

    void DestroyModel(){
        loadRandomModelInstance.DestroyInstantiatedObject(); 
    }

    public void ShowPrefab()
    {
        helpPanel.SetActive(true); // Set the prefab's active state to false
    }

    public void TimeCounter(){
        timer.StartTimer(); 
    }

    void CheckControlType()
    {
        if (CommConstants.control_type == 1)
        {
            if (model.transform.childCount > 0)
            {
                Transform childTransform = model.transform.GetChild(0);

                // Get the MouseRotate component attached to the first child
                MouseRotate mouseRotateComponent = childTransform.GetComponent<MouseRotate>();

                // Check if the component exists before attempting to remove it
                if (mouseRotateComponent != null)
                {
                    // Remove the MouseRotate component from the first child
                    Destroy(mouseRotateComponent);
                    // Or use DestroyImmediate(mouseRotateComponent) if you want to remove it immediately
                }
            }
            else
            {
                Debug.Log("No child found at index 0 in model.transform");
            }
        }
        else if (CommConstants.control_type == 2)
        {
            GameObject.Find("SideSlider").SetActive(false);
            GameObject.Find("BottomSlider").SetActive(false);
        }
        else if (CommConstants.control_type == 3)
        {
            // TO DO 
        }
        else
        {
            Debug.Log("Invalid type");
        }
    }


    IEnumerator AddToDB(float time, int ctrl_type){

        WWWForm form = new WWWForm();
        form.AddField("rotate_time", time.ToString());
        form.AddField("control_type", ctrl_type.ToString());
        

        using (UnityWebRequest www = UnityWebRequest.Post(CommConstants.ServerURL+"/quiz_view.php", form)){

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
                    
                    Debug.Log(www.downloadHandler.text);
                    if(www.downloadHandler.text == "400"){
                        Debug.Log("Bad Request");
                    }
                    else if(www.downloadHandler.text == "1"){ //POPRAVITE DEBUG  
                        Debug.Log("Table updated");  
                    }else{
                        Debug.Log(www.downloadHandler.text);
                    }
                   
                }
        }
    }
    
}