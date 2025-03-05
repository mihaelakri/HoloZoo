using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance { get; private set; }
    string previousScene;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Animal_list")
        {
            Button btn = GameObject.Find("Back").GetComponent<Button>();

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene((string)previousScene.Clone());
            });
        }
        else if (scene.name != "HologramAnimalMobile")
        {
            previousScene = scene.name;
        }
    }
}