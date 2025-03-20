using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HoloZoo.DataModels;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine.Networking;
using System.Collections;
using System;
using UnityEditor;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }
    public static bool isMainDataLoaded = false;
    public static bool isUserDataLoaded = false;

    readonly JsonSerializerSettings jsonSettings = new()
    {
        MissingMemberHandling = MissingMemberHandling.Error,
        ContractResolver = new RequireAllPropertiesResolver()
    };

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
            Destroy(gameObject);
    }

    void Start()
    {
        LoadGameData();
    }

    public void LoadGameData()
    {
        if (PlayerPrefs.HasKey("lang"))
            StartCoroutine(LoadTranslatedTables(PlayerPrefs.GetString("lang", "en")));
        else
            isMainDataLoaded = true;
        StartCoroutine(LoadUserTable());
    }

    IEnumerator FetchText(string path, Action<string> callback)
    {
        using UnityWebRequest unityWebRequest = UnityWebRequest.Get(path);
        var request = unityWebRequest.SendWebRequest();

        yield return request;

        callback?.Invoke(unityWebRequest.downloadHandler.text);
    }

    // Editable files cannot be stored in `Application.streamingAssetsPath`
    IEnumerator LoadUserTable()
    {
        string user_json = null;
        if (!File.Exists(Application.persistentDataPath + "/Users.json"))
        {
            yield return StartCoroutine(FetchText(Application.streamingAssetsPath + "/Users.json", jsonResp =>
            {
                user_json = jsonResp;
            }));
            users = JsonConvert.DeserializeObject<List<User>>(user_json, jsonSettings);
            Debug.Log($"{nameof(GameData)}: Users.json missing, creating with: {user_json}");
            SaveUserData();
        }
        else
        {
            user_json = File.ReadAllText(Application.persistentDataPath + "/Users.json");
            users = JsonConvert.DeserializeObject<List<User>>(user_json, jsonSettings);
        }
        isUserDataLoaded = true;
    }

    public IEnumerator LoadTranslatedTables(string lang)
    {
        string animal_main, animal_text, question_main, question_text, area_json, translation_json;
        animal_main = animal_text = question_main = question_text = area_json = translation_json = null;

        yield return StartCoroutine(FetchText(Application.streamingAssetsPath + "/Animals.json", jsonResp =>
        {
            animal_main = jsonResp;
        }));
        yield return StartCoroutine(FetchText(Application.streamingAssetsPath + $"/Animal_text_{lang}.json", jsonResp =>
        {
            animal_text = jsonResp;
        }));
        animals = MergeJArrays<Animal>(animal_main, animal_text, "id_animal");

        yield return StartCoroutine(FetchText(Application.streamingAssetsPath + "/Questions.json", jsonResp =>
        {
            question_main = jsonResp;
        }));
        yield return StartCoroutine(FetchText(Application.streamingAssetsPath + $"/Questions_text_{lang}.json", jsonResp =>
        {
            question_text = jsonResp;
        }));
        questions = MergeJArrays<Question>(question_main, question_text, "id_question");

        yield return StartCoroutine(FetchText(Application.streamingAssetsPath + "/Areas.json", jsonResp =>
        {
            area_json = jsonResp;
        }));

        areas = JsonConvert.DeserializeObject<List<Area>>(
            area_json,
            new JsonSerializerSettings
            {
                ContractResolver = new CustomPropertyResolver(new Dictionary<string, string> {
                    {
                        "name", $"name_{lang}"
                    },
                })
            }
        );

        yield return StartCoroutine(FetchText(Application.streamingAssetsPath + $"/Translations_{lang}.json", jsonResp =>
        {
            translation_json = jsonResp;
        }));
        translations = JsonConvert.DeserializeObject<Translations>(translation_json, jsonSettings);

        isMainDataLoaded = true;
    }

    public void SaveUserData()
    {
        string updatedJson = JsonConvert.SerializeObject(users);
        File.WriteAllText(Application.persistentDataPath + "/Users.json", updatedJson);
        Debug.Log($"{nameof(GameData)}: User data saved: {updatedJson}");
    }

    public class RequireAllPropertiesResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            prop.Required = Required.Always;
            return prop;
        }
    }

    class CustomPropertyResolver : RequireAllPropertiesResolver
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
        if (id == 0)    // Special-case for Globe
            return new Animal()
            {
                id = 0,
            };
        return animals.FirstOrDefault(a => a.id == id);
    }

    public List<Animal> GetAnimalNames(int levelCap)
    {
        var result = animals.Where(a => a.level <= levelCap).ToList();
        Debug.Log($"{nameof(GameData)} - {nameof(GetAnimalNames)}, found {result.Count} animals");
        return result;
    }

    public List<Animal> GetAreaAnimals(Area area)
    {
        var result = animals.Where(a => a.id_area.Contains(area.id)).ToList();
        Debug.Log($"{nameof(GameData)} - {nameof(GetAreaAnimals)}, found {result.Count} animals");
        return result;
    }

    public List<Animal> GetAnimalsAtLevel(int level)
    {
        var result = animals.Where(a => a.level == level).ToList();
        Debug.Log($"{nameof(GameData)} - {nameof(GetAnimalsAtLevel)}, found {result.Count} animals");
        return result;
    }

    public Area? GetArea(int id)
    {
        return areas.FirstOrDefault(a => a.id == id);
    }

    public User CreateUser(string username, string password)
    {
        int maxUserId = users.OrderByDescending(user => user.id).First().id;
        User user = new()
        {
            id = maxUserId + 1,
            level = 1,
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
        return users.FirstOrDefault(u => u.id == id);
    }

    public User? GetCurrentUser()
    {
        return GetUser(PlayerPrefs.GetInt("ID", -1));
    }

    public bool UpdateUserExperience(int correctAnswers, User user)
    {
        int xpPerLevel = 100;
        int expTotal = user.experience + (correctAnswers * 10);
        user.experience = expTotal % xpPerLevel;
        user.level += expTotal / xpPerLevel;

        SaveUserData();
        return expTotal >= xpPerLevel;
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
        var userLevel = GetCurrentUser()?.level ?? int.MinValue;

        var questions_lvlcapped = questions.Where(q =>
            (GetAnimal(q.id_animal)?.level ?? int.MaxValue) <= userLevel
        ).ToList();

        return ShuffleList(questions_lvlcapped).GetRange(0, numQuestions);
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
                obj2.Remove(foreignKey);
                mergedObject.Merge(obj2);
            }
            else
            {
                Debug.LogError($"MergeJArrays failed to foreign key: {id}");
            }

            mergedArray.Add(mergedObject);
        }

        List<T1> mergedList = mergedArray.ToObject<List<T1>>(JsonSerializer.Create(jsonSettings));
        // Debug.Log(JsonConvert.SerializeObject(mergedList, Formatting.Indented));

        return mergedList;
    }
}