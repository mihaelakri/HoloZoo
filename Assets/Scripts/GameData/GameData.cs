using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HoloZoo.DataModels;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    public List<Area> areas;
    public List<Animal> animals;
    public List<Question> questions;
    public List<User> users;
    public Translations translations;

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

    void Start()
    {
        LoadGameData(PlayerPrefs.GetString("lang", "en"));
    }

    public void LoadGameData(string lang)
    {
        LoadTranslatedTables(lang);
        CopyEditableFiles();
        users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(Application.persistentDataPath + "/Users.json"));
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
                File.Copy(Application.streamingAssetsPath + filePath, Application.persistentDataPath + filePath);
                Debug.Log($"{nameof(GameData)}: Copied {filePath} to persistent");
            }
        }
    }

    public void LoadTranslatedTables(string lang)
    {
        string animal_main = File.ReadAllText(Application.streamingAssetsPath + "/Animals.json");
        string animal_text = File.ReadAllText(Application.streamingAssetsPath + $"/Animal_text_{lang}.json");
        animals = MergeJArrays<Animal>(animal_main, animal_text, "id_animal");

        string question_main = File.ReadAllText(Application.streamingAssetsPath + "/Questions.json");
        string question_text = File.ReadAllText(Application.streamingAssetsPath + $"/Questions_text_{lang}.json");
        questions = MergeJArrays<Question>(question_main, question_text, "id_question");

        areas = JsonConvert.DeserializeObject<List<Area>>(
            File.ReadAllText(Application.streamingAssetsPath + "/Areas.json"),
            new JsonSerializerSettings
            {
                ContractResolver = new CustomPropertyResolver(new Dictionary<string, string> {
                    {
                        "name", $"name_{lang}"
                    },
                })
            }
        );
        // Debug.Log(JsonConvert.SerializeObject(areas, Formatting.Indented));

        translations = JsonConvert.DeserializeObject<Translations>(File.ReadAllText(Application.streamingAssetsPath + $"/Translations_{lang}.json"));
    }

    public void SaveUserData()
    {
        string updatedJson = JsonUtility.ToJson(users, true);
        File.WriteAllText(Application.persistentDataPath + "/Users.json", updatedJson);
        Debug.Log($"{nameof(GameData)}: User data saved");
    }

    class CustomPropertyResolver : DefaultContractResolver
    {
        private readonly Dictionary<string, string> _propertyMappings;

        public CustomPropertyResolver(Dictionary<string, string> propertyMappings)
        {
            _propertyMappings = propertyMappings;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return _propertyMappings.TryGetValue(propertyName, out string resolvedName) ? resolvedName : propertyName;
        }
    }

#nullable enable
    public Animal? GetAnimal(int id)
    {
        return animals.First(a => a.id == id);
    }

    public List<Animal>? GetAnimalNames(int levelCap)
    {
        return animals.Where(a => a.level < levelCap).ToList();
    }

    public List<Animal>? GetAreaAnimals(Area area)
    {
        return animals.Where(a => a.id_area.Contains(area.id)).ToList();
    }

    public Area? GetArea(int id)
    {
        return areas.First(a => a.id == id);
    }

    public User CreateUser(string username, string password)
    {
        int maxUserId = users.OrderByDescending(user => user.id).First().id;
        User user = new()
        {
            id = maxUserId + 1,
            level = 0,
            username = username,
            // TODO hash this, esp. if not stored locally in future
            password = password,
            experience = 0,
        };
        users.Add(user);
        SaveUserData();
        return user;
    }

    public User? GetUser(int id)
    {
        return users.First(u => u.id == id);
    }

    public User? GetCurrentUser()
    {
        return GetUser(PlayerPrefs.GetInt("ID", -1));
    }

    public void UpdateUserExperience(int correctAnswers, User user)
    {
        user.experience = correctAnswers * 10;
        SaveUserData();
    }

    public void UpdateUserPassword(string password, User user)
    {
        user.password = password;
        SaveUserData();
    }

    public enum CredentialResponse
    {
        WrongUsername,
        WrongPassword,
        Success
    }

    public (User?, CredentialResponse) CheckUserCredentials(string username, string password)
    {
        var user = users.Where(u => u.username == username).DefaultIfEmpty(null).First();

        if (user == null)
        {
            Debug.Log($"{nameof(GameData)} - Username non-existent");
            return (user, CredentialResponse.WrongUsername);
        }
        else if (user.password != password)
        {
            Debug.Log($"{nameof(GameData)} - Wrong password");
            return (user, CredentialResponse.WrongPassword);
        }

        return (user, CredentialResponse.Success);
    }

    public List<Question> GetQuizQuestions(int difficulty)
    {
        int numQuestions = difficulty * 3;
        return ShuffleList(questions).GetRange(0, numQuestions);
    }

#nullable restore

    List<T> ShuffleList<T>(List<T> list)
    {
        // Out-of-place Fisher-Yates Shuffle Algorithm
        List<T> listCopy = new(list);
        System.Random rng = new();
        int n = listCopy.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (listCopy[k], listCopy[n]) = (listCopy[n], listCopy[k]);
        }
        return listCopy;
    }

    public List<T1> MergeJArrays<T1>(string mainArray, string dependentArray, string foreignKey)
    {
        JArray array1 = JArray.Parse(mainArray);
        JArray array2 = JArray.Parse(dependentArray);

        Dictionary<int, JObject> foreignKeyLookup = array2
            .Select(obj =>
            {
                JObject jObj = (JObject)obj;
                jObj.Remove("id");
                return jObj;
            })
            .ToDictionary(obj => obj[foreignKey].Value<int>());

        JArray mergedArray = new();

        // Merge objects using id as key
        foreach (JObject obj1 in array1)
        {
            int id = obj1["id"].Value<int>();
            JObject mergedObject = new(obj1);

            if (foreignKeyLookup.TryGetValue(id, out JObject obj2))
            {
                mergedObject.Merge(obj2);
            }
            else
            {
                Debug.LogError($"MergeJArrays failed to foreign key: {id}");
            }

            mergedArray.Add(mergedObject);
        }

        List<T1> mergedList = mergedArray.ToObject<List<T1>>();
        // Debug.Log(JsonConvert.SerializeObject(mergedList, Formatting.Indented));

        return mergedList;
    }
}