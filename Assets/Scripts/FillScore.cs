using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillScore : MonoBehaviour
{
    public Text scoreText;
    public Text message;
    public Sprite happyFace;
    public Sprite sadFace;
    public Image face;
    public Font liberationFont;
    public Font jostFont;

    private Dictionary<string, Dictionary<string, string>> messages = new Dictionary<string, Dictionary<string, string>>
    {
        { "en", new Dictionary<string, string> { { "try_again", "Try again!" }, { "bravo", "Bravo!" } } },
        { "fr", new Dictionary<string, string> { { "try_again", "Essaie encore !" }, { "bravo", "Félicitations !" } } },
        { "hr", new Dictionary<string, string> { { "try_again", "Pokušaj ponovo!" }, { "bravo", "Bravo!" } } },
        { "es", new Dictionary<string, string> { { "try_again", "¡Inténtalo de nuevo!" }, { "bravo", "¡Bravo!" } } },
        { "hu", new Dictionary<string, string> { { "try_again", "Próbáld újra!" }, { "bravo", "Bravó!" } } }
    };

    void Start()
    {
        string selectedLanguage = PlayerPrefs.GetString("lang", "en");
        scoreText.text = PlayerPrefs.GetInt("Score") + "/" + PlayerPrefs.GetInt("QuestionCount");
        if (PlayerPrefs.GetInt("Score") / (float)PlayerPrefs.GetInt("QuestionCount") < 0.5f)
        {
            message.text = messages[selectedLanguage]["try_again"];
            face.GetComponent<Image>().sprite = sadFace;
        }
        else
        {
            message.text = messages[selectedLanguage]["bravo"];
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
