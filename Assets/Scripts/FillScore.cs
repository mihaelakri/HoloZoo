using System.Collections;
using System.Linq;
using HoloZoo.DataModels;
using UnityEngine;
using UnityEngine.UI;

public class FillScore : MonoBehaviour
{
    public Text scoreText;
    public Text message;
    public Sprite happyFace;
    public Sprite sadFace;
    public Image face;

    public GameObject notificationContainer;
    public GameObject animalContainer;
    public GameObject animalProfilePrefab;
    public GameObject buttonOk;
    public GameObject notificationHeader;

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
        bool userLeveledUp = GameData.Instance.UpdateUserExperience(PlayerPrefs.GetInt("Score"), user);

        if (userLeveledUp)
            ShowUnlockedAnimals(user);

        yield break;
    }

    void ShowUnlockedAnimals(User user)
    {
        Debug.Log($"{nameof(FillScore)} - ShowUnlockedAnimals");

        notificationContainer.SetActive(true);
        
        buttonOk.transform.GetComponent<Text>().text = GameData.Instance.translations.buttons.btn_ok;
        // TODO get real translations for this
        notificationHeader.transform.GetComponent<Text>().text = GameData.Instance.translations.score_scene.bravo;

        var newAnimals = GameData.Instance.GetAnimalsAtLevel(user.level);
        foreach (var animal in newAnimals)
        {
            var animalProfile = Instantiate(animalProfilePrefab, animalContainer.transform);

            Text animalNamePrefab = animalProfile.transform.GetComponentInChildren<Text>();
            Image animalImagePrefab = animalProfile.transform.GetComponentInChildren<Image>();

            animalNamePrefab.color = Color.black;
            animalNamePrefab.text = animal.name;

            Texture2D myTexture = Resources.Load<Texture2D>(animal.url_slika);
            animalImagePrefab.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2());
        }
    }
}