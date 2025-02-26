using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HoloZoo.DataModels;

public class GameData : MonoBehaviour
{
    private static GameData Instance;

    public List<Animal_text> animal_text;
    public List<Animal> animals;
    public List<Area> areas;
    public List<Question_text> question_text;
    public List<Question> questions;
    public List<User> users;

    public void LoadGameData(string lang)
    {
        animals = JsonUtility.FromJson<List<Animal>>(File.ReadAllText(Application.streamingAssetsPath + "/Animals.json"));
        animal_text = JsonUtility.FromJson<List<Animal_text>>(File.ReadAllText(Application.streamingAssetsPath + $"/Animal_text_{lang}.json"));
        areas = JsonUtility.FromJson<List<Area>>(File.ReadAllText(Application.streamingAssetsPath + "/Areas.json"));
        questions = JsonUtility.FromJson<List<Question>>(File.ReadAllText(Application.streamingAssetsPath + "/Questions.json"));
        question_text = JsonUtility.FromJson<List<Question_text>>(File.ReadAllText(Application.streamingAssetsPath + $"/Questions_text_{lang}.json"));

        CopyEditableFiles();
        users = JsonUtility.FromJson<List<User>>(File.ReadAllText(Application.persistentDataPath + "/Users.json"));
    }

    // Editable files cannot be stored in `Application.streamingAssetsPath`
    void CopyEditableFiles()
    {
        List<string> filePaths = new() {
            "/Users.json",
        };
        foreach (string filePath in filePaths)
        {
            if (!File.Exists(Application.persistentDataPath + filePath))
            {
                File.WriteAllText(Application.persistentDataPath + filePath, Application.streamingAssetsPath + filePath);
                Debug.Log($"{nameof(GameData)}: Copied {filePath} to persistent");
            }
        }
    }

    public void SwitchLanguage(string lang)
    {
        animal_text = JsonUtility.FromJson<List<Animal_text>>(File.ReadAllText(Application.streamingAssetsPath + $"/Animal_text_{lang}.json"));
        question_text = JsonUtility.FromJson<List<Question_text>>(File.ReadAllText(Application.streamingAssetsPath + $"/Questions_text_{lang}.json"));
    }

    public void SaveUserData()
    {
        string updatedJson = JsonUtility.ToJson(users, true);
        File.WriteAllText(Application.persistentDataPath + "/Users.json", updatedJson);
        Debug.Log($"{nameof(GameData)}: User data saved");
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGameData(PlayerPrefs.GetString("lang", "en"));
    }
}