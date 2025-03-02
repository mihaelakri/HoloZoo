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
        Debug.Log("OnSceneLoaded: " + scene.name);

        if (scene.name == "Animal_list")
        {
            Button btn = GameObject.Find("Back").GetComponent<Button>();

            string previousScene_copy = previousScene;  // prevents pass-by-ref bug
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(previousScene_copy);
                // Debug.Log($"{nameof(SceneManagement)} - listener prev_scene: {previousScene_copy}");
            });

            // Debug.Log($"{nameof(SceneManagement)} - {previousScene} set as previous scene for {scene.name}");
        }
        else if (scene.name != "HologramAnimalMobile")
        {
            previousScene = scene.name;
        }
    }
}