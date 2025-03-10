using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance { get; private set; }
    private readonly Stack<string> sceneStack = new();

    void Awake()
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
        if (scene.name == "AnimalProfile")
        {
            Button btn = GameObject.Find("BackButton").GetComponent<Button>();

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene((string)sceneStack.Peek().Clone());
            });
        }

        if (sceneStack.Count == 0 || sceneStack.Peek() != scene.name)
        {
            sceneStack.Push(scene.name);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackButton();
        }
    }

    public void HandleBackButton()
    {
        if (sceneStack.Count <= 1)
        {
            Debug.Log($"{nameof(SceneManagement)} - Quitting application, sceneStack count: {sceneStack.Count}");
            Application.Quit();
        }
        sceneStack.Pop(); // Remove current scene
        string prevScene = sceneStack.Peek();
        SceneManager.LoadScene(prevScene);
        Debug.Log($"{nameof(SceneManagement)} - Loading prev scene: {prevScene}");
    }

    public void ClearBackstack()
    {
        sceneStack.Clear();
        Debug.Log($"{nameof(SceneManagement)} - Backstack Cleared");
    }
}