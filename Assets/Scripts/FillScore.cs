using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FillScore : MonoBehaviour
{
    public Text scoreText;
    public Text message;
    public Sprite happyFace;
    public Sprite sadFace;
    public Image face;

    void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("Score") + "/" + PlayerPrefs.GetInt("QuestionCount");
        var translation = GameData.Instance.translations;
        if (PlayerPrefs.GetInt("Score") / (float)PlayerPrefs.GetInt("QuestionCount") < 0.5f)
        {
            message.text = translation.score_scene.try_again;
            face.GetComponent<Image>().sprite = sadFace;
        }
        else
        {
            message.text = translation.score_scene.bravo;
            face.GetComponent<Image>().sprite = happyFace;
        }
        StartCoroutine(SetExperience());
    }

    IEnumerator SetExperience()
    {
        var user = GameData.Instance.GetCurrentUser();
        GameData.Instance.UpdateUserExperience(PlayerPrefs.GetInt("Score"), user);

        yield break;
    }
}