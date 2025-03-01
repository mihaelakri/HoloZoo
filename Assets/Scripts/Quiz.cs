using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using HoloZoo.DataModels;

public class Quiz : MonoBehaviour
{
    public Text questionText;
    private List<Question> questions;
    public int questionsCount;
    public int questionsCounter;

    public Sprite imgCorrect;
    public Sprite imgWrong;
    public Sprite imgWrongOther;
    public Sprite imgNeutral;

    public GameObject answerParent;
    private Button[] answerButtons;
    public Button nextButton;

    private string selectedLanguage;
    private int correctAnswerCount;

    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                { "next_question", "Next question" },
                { "see_results", "See results" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "next_question", "Prochaine question" },
                { "see_results", "Voir les résultats" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "next_question", "Sljedeće pitanje" },
                { "see_results", "Vidi rezultat" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "next_question", "Próxima pregunta" },
                { "see_results", "Ver resultados" }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "next_question", "Következő kérdés" },
                { "see_results", "Lásd az eredményeket" }
            }
        }
    };

    // Start is called before the first frame update
    void Start()
    {
        selectedLanguage = PlayerPrefs.GetString("lang", "en");
        answerButtons = answerParent.GetComponentsInChildren<Button>();

        StartCoroutine(FillQuestion());
    }

    IEnumerator FillQuestion()
    {
        questions = GameData.Instance.GetQuizQuestions(PlayerPrefs.GetInt("diff"));
        questionsCount = questions.Count;
        questionsCounter = 0;

        FillDataInUI();

        yield break;
    }

    public void CheckAnswer()
    {
        ColorButtons();

        questionsCounter++;

        nextButton.gameObject.SetActive(true);
        nextButton.transform.LeanMoveLocal(new Vector2(123, -225), 1).setEaseOutQuart();
        if (questionsCounter < questionsCount)
            nextButton.transform.GetChild(0).GetComponent<Text>().text = translations[selectedLanguage]["next_question"];
        else
            nextButton.transform.GetChild(0).GetComponent<Text>().text = translations[selectedLanguage]["see_results"];
    }

    public void nextQuestion()
    {
        nextButton.gameObject.SetActive(false);
        nextButton.transform.LeanMoveLocal(new Vector2(-27, -446), 1).setEaseOutQuart();

        if (questionsCounter >= questionsCount)
        {
            PlayerPrefs.SetInt("Score", correctAnswerCount);
            PlayerPrefs.SetInt("QuestionCount", questionsCount);
            SceneManager.LoadScene("Score");
            return;
        }

        foreach (Button button in answerButtons)
        {
            button.GetComponent<Image>().sprite = imgNeutral;
            button.enabled = true;
        }

        FillDataInUI();
    }

    private void FillDataInUI()
    {
        questionText.text = questions[questionsCounter].question_text;
        answerButtons[0].GetComponentInChildren<Text>().text = questions[questionsCounter].answer_one;
        answerButtons[1].GetComponentInChildren<Text>().text = questions[questionsCounter].answer_two;
        answerButtons[2].GetComponentInChildren<Text>().text = questions[questionsCounter].answer_three;
        GameObject.Find("AccessibilityManager").GetComponent<ApplyAccessibility>().ApplyAccessibilitySettings();
    }

    private void ColorButtons()
    {
        int correct = questions[questionsCounter].correct_answer;
        int chosen = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex() + 1;

        foreach (Button button in answerButtons)
        {
            button.enabled = false;

            int btnIndex = button.transform.GetSiblingIndex() + 1;

            if (btnIndex == correct)
            {
                button.GetComponent<Image>().sprite = imgCorrect;
                if (btnIndex == chosen)
                    correctAnswerCount++;
            }
            else if (btnIndex == chosen && btnIndex != correct)
            {
                button.GetComponent<Image>().color = Color.white;
                button.GetComponent<Image>().sprite = imgWrong;
            }
            else
            {
                button.GetComponent<Image>().color = Color.white;
                button.GetComponent<Image>().sprite = imgWrongOther;
            }
        }
    }
}