using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;

public class ApplyAccessibility : MonoBehaviour
{
    public static ApplyAccessibility Instance; // Singleton pristup

    public Font openDyslexic;
    public Font jostFont;
    public GameObject accessibilityDummy;

    List<object> taggedObjects;
    Text[] textObjects;

    private class TaggedObjects<T1>
        where T1 : Graphic
    {
        public string tag;
        public Func<string, Tuple<T1, Color>[]> fetchAction;
        public Color contrastColor;
        public Tuple<T1, Color>[] objects;

        public TaggedObjects(string tag, Func<string, Tuple<T1, Color>[]> fetchAction, Color contrastColor)
        {
            this.tag = tag;
            this.fetchAction = fetchAction;
            this.contrastColor = contrastColor;
        }

        public void FetchObjects()
        {
            objects = fetchAction(tag);
        }

        public void ApplyContrast(bool isContrastEnabled)
        {
            foreach (var obj in objects)
            {
                // Ensure original color is retained when contrast is disabled
                Color color = isContrastEnabled ? contrastColor : obj.Item2;
                obj.Item1.color = color;
            }
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Održava postavke između scena
        }
        else
            Destroy(gameObject);

        taggedObjects = new()
        {
            new TaggedObjects<Text>(
                "text-dark",
                (_) => GetTextsWithTag(_),
                Color.black
            ),
            new TaggedObjects<Text>(
                "text-light",
                (_) => GetTextsWithTag(_),
                Color.white
            ),
            new TaggedObjects<Image>(
                "img-dark",
                (_) => GetImagesWithTag(_),
                Color.black
            ),
            new TaggedObjects<Image>(
                "img-light",
                (_) => GetImagesWithTag(_),
                Color.white     // Currently does nothing, should be implemented possibly via a shader
            ),
            new TaggedObjects<Image>(
                "img-mid",
                (_) => GetImagesWithTag(_),
                Color.grey
            )
        };
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Hack to ensure the rest is called during Starts in new objects
        Instantiate(accessibilityDummy);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private Tuple<Text, Color>[] GetTextsWithTag(string tag)
    {
        return GameObject.FindGameObjectsWithTag(tag).Select(o =>
        {
            Text txt = o.GetComponent<Text>();
            return new Tuple<Text, Color>(txt, txt.color);
        }).ToArray();
    }

    private Tuple<Image, Color>[] GetImagesWithTag(string tag)
    {
        return GameObject.FindGameObjectsWithTag(tag).Select(o =>
        {
            if (!o.TryGetComponent<Image>(out var img))
            {
                // Some buttons have Image component as a child
                img = o.GetComponentInChildren<Image>();
            }

            if (img == null)
                Debug.Log("Scene: [" + SceneManager.GetActiveScene().name + "] Object: [" + o.name + "] doesn't have an image component");

            return new Tuple<Image, Color>(img, img.color);
        }).ToArray();
    }

    public void LoadObjects()
    {
        foreach (var taggedObject in taggedObjects)
        {
            if (taggedObject is TaggedObjects<Text> taggedTextObject)
                taggedTextObject.FetchObjects();
            else if (taggedObject is TaggedObjects<Image> taggedImageObject)
                taggedImageObject.FetchObjects();
        }
        textObjects = taggedObjects.Where(e => e is TaggedObjects<Text>)
            .SelectMany(e => ((TaggedObjects<Text>)e).objects.Select(o => o.Item1))
            .ToArray();
    }

    public void ApplyAccessibilitySettings()
    {
        int fontSize = PlayerPrefs.GetInt("font_size", 14);
        bool isDyslexiaEnabled = PlayerPrefs.GetInt("dyslexia", 0) == 1;
        bool isContrastEnabled = PlayerPrefs.GetInt("contrast", 0) == 1;

        foreach (Text text in textObjects)
        {
            text.fontSize = fontSize;
            text.font = isDyslexiaEnabled ? openDyslexic : jostFont;
        }

        foreach (var taggedObject in taggedObjects)
        {
            if (taggedObject is TaggedObjects<Text> taggedTextObject)
                taggedTextObject.ApplyContrast(isContrastEnabled);
            else if (taggedObject is TaggedObjects<Image> taggedImageObject)
                taggedImageObject.ApplyContrast(isContrastEnabled);
        }
    }

    public void LoadAndStyle()
    {
        LoadObjects();
        ApplyAccessibilitySettings();
    }
}