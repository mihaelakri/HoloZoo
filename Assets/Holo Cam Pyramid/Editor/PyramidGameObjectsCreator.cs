using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

/// <summary>
/// Adds menu items to GameObject/Create menu to allow user add important Pyramid prefabs effortlessly
/// </summary>
public class PyramidGameObjectsCreator : Editor {

    private static string PrefabFolderRoot = "Assets/Holo Cam Pyramid/Prefabs/";
    private static string PrefabCameraName = "HologramCamera";
    private static string PrefabVisualizerName = "PyramidVisualizer";
    private static string PrefabGuideLinesName = "PyramidGuideLines";
    private static string PrefabBackgroundImgName = "BackgroundImage";

    /// <summary>
    /// Creates all related objects of the hologram camera in a proper way
    /// </summary>
    /// <param name="menuCommand"></param>
    [MenuItem("GameObject/Hologram Camera", false, 10)]
    public static void CreateHoloCameraRelatedGameObjects(MenuCommand menuCommand)
    {
        CreateHoloCamera(menuCommand);
        Undo.SetCurrentGroupName("Create Hologram Camera");
        CreateBGImage();
        CreateHoloRenderer();
        CreateGuideLines();
    }

    /// <summary>
    /// It is the Validate function of the "GameObject > Hologram Camera" menu item.
    /// </summary>
    /// <returns>Can "GameObject > Hologram Camera" be enabled?</returns>
    [MenuItem("GameObject/Hologram Camera", true, 10)]
    public static bool CanCreateCameraRelatedGameObjects()
    {
        bool CanCreate = false;

        // Checks Holo Camera game objects had not been created yet by the user
        if (GameObject.Find(PrefabCameraName) == null &&
            GameObject.Find(PrefabVisualizerName) == null &&
            GameObject.Find(PrefabGuideLinesName) == null)
        {
            CanCreate = true;
        }


        return CanCreate;
    }

    /// <summary>
    /// Creates Holo Camera prefab instance in active scene
    /// </summary>
    /// <param name="menuCommand">Used to extract the context for a MenuItem.</param>
    // [MenuItem("GameObject/Hologram Pyramid/Camera", false, 10)]
	private static void CreateHoloCamera(MenuCommand menuCommand)
    {
        // Loads HoloCamera prefab from asset database
        Object pyramidPrefab = AssetDatabase.LoadAssetAtPath(PrefabFolderRoot + PrefabCameraName + ".prefab", typeof(Object));
        // References the instanced prefab
        GameObject go = PrefabUtility.InstantiatePrefab(pyramidPrefab as GameObject) as GameObject;
        if (go == null)
            return; // It does nothing if prefab can not be instanced

        // It gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Registers the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create Pyramid Camera");
        // Selects the instanced prefab
        Selection.activeObject = go;
    }    

    /// <summary>
    /// Creates a Visualizer prefab instance in scene for show content being viewed by the Holo Camera
    /// </summary>
    /// <param name="menuCommand">Used to extract the context for a MenuItem.</param>
    // [MenuItem("GameObject/Hologram Pyramid/Visualizer", false, 10)]
    private static void CreateHoloRenderer()
    {
        // Assumes canvas exists before the click
        bool ExistsCanvasBeforeClick = true;


        // Ensures it exists an canvas element in current scene
        if (GameObject.Find("Canvas") == null)
        {
            // Creates a canvas element and his EventListener
            CreateCanvas();
            ExistsCanvasBeforeClick = false;
        }

        // Loads HoloVisualizer prefab from asset database
        Object visualizerPrefab = AssetDatabase.LoadAssetAtPath(PrefabFolderRoot + PrefabVisualizerName + ".prefab", typeof(Object));
        // References the instanced prefab
        GameObject go = PrefabUtility.InstantiatePrefab(visualizerPrefab) as GameObject;
        if (go == null)
            return; // Return if prefab can not be instanced

        /*
         * Casting is done with "as" operator because it returns null when cast
         * can't be made 
         */

        // Makes instanced prefab child of the created canvas element
        go.transform.SetParent(GameObject.Find("Canvas").transform);


        // GameObject gameObjectContext = menuCommand.context as GameObject;

        /*if (ExistsCanvasBeforeClick && gameObjectContext != null)
        {
            // If parent (or any of its parents) of target of context click have a canvas component 
            if (gameObjectContext.GetComponentInParent<Canvas>() != null)
            {
                // Ensures it gets reparented if this was a context click (otherwise does nothing)
                GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            }
        }
        else
        {
            // Makes instanced prefab child of the created canvas element
            go.transform.SetParent(GameObject.Find("Canvas").transform);
        }*/


        // Moves visualizer to canvas center
        go.transform.localPosition = new Vector3(0, 0, 0);
        // Registers the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create Pyramid Visualizer");
    }

    /// <summary>
    /// Creates a Pyramid Guide Line prefab instance in scene for help the user to positioning the pyramid over screen.
    /// </summary>
    /// <param name="menuCommand">Used to extract the context for a MenuItem.</param>
    // [MenuItem("GameObject/Hologram Pyramid/Guide Lines", false, 10)]
    private static void CreateGuideLines()
    {
        bool ExistsCanvasBeforeClick = true;
        
        // Ensures it exists an canvas element in current scene
        if (GameObject.Find("Canvas") == null)
        {
            // Creates a canvas element and his EventListener
            CreateCanvas();
            ExistsCanvasBeforeClick = false;
        }

        // Loads PyramidGuideLine prefab from asset database
        Object guideLinePrefab = AssetDatabase.LoadAssetAtPath(PrefabFolderRoot + PrefabGuideLinesName + ".prefab", typeof(Object));
        // References the instanced prefab
        GameObject go = PrefabUtility.InstantiatePrefab(guideLinePrefab) as GameObject;
        if (go == null)
            return; // Return if prefab can not be instanced

        go.transform.SetParent(GameObject.Find("Canvas").transform);

        /*GameObject gameObjectContext = menuCommand.context as GameObject;

        if (ExistsCanvasBeforeClick && gameObjectContext != null)
        {
            // If parent (or any of its parents) of target of context click have a canvas component 
            if (gameObjectContext.GetComponentInParent<Canvas>() != null)
            {
                // It gets reparented if this was a context click (otherwise does nothing)
                GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            }
        }
        else
        {
            // Makes instanced prefab child of the created canvas element
            go.transform.SetParent(GameObject.Find("Canvas").transform);
        }*/


        // Moves guide to canvas center
        go.transform.localPosition = new Vector3(0, 0, 0);
        // Registers the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create Pyramid Guide Lines");
    }

    /// <summary>
    /// Creates Background Image for the holographic visualizer
    /// </summary>
    private static void CreateBGImage()
    {
        bool ExistsCanvasBeforeClick = true;

        // Ensures it exists an canvas element in current scene
        if (GameObject.Find("Canvas") == null)
        {
            // Creates a canvas element and his EventListener
            CreateCanvas();
            ExistsCanvasBeforeClick = false;
        }

        // Loads PyramidGuideLine prefab from asset database
        Object guideLinePrefab = AssetDatabase.LoadAssetAtPath(PrefabFolderRoot + PrefabBackgroundImgName + ".prefab", typeof(Object));
        // References the instanced prefab
        GameObject go = PrefabUtility.InstantiatePrefab(guideLinePrefab) as GameObject;
        if (go == null)
            return; // Return if prefab can not be instanced

        go.transform.SetParent(GameObject.Find("Canvas").transform);

        // Moves visualizer to canvas center
        go.transform.localPosition = new Vector3(0, 0, 0.5f);
        // Registers the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create Pyramid Visualizer");
    }

    /// <summary>
    /// Creates an Canvas and a EventLister configured as the default canvas. 
    /// </summary>
    private static void CreateCanvas()
    {
        // Creates a GameObject called "Canvas"
        GameObject canvasGO = new GameObject("Canvas");
        // Sets Canvas GameObject layer to UI (index 5)
        canvasGO.layer = 5;
        // Adds RectTransform component to Canvas GameObject
        canvasGO.AddComponent<RectTransform>();
        // Adds Canvas component to Canvas GameObject and save a reference of the component
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        // Sets the Canvas component render mode property to ScreenSpaceOverlay
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Adds CanvasScaler component to Canvas GameObject and save a reference of the component
        CanvasScaler canvasScaler = canvasGO.AddComponent<CanvasScaler>();
        // Sets uiScaleMode property of CanvasScaler component to ScaleWithScreenSize
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        // Adds GraphicRaycaster component to Canvas GameObject
        canvasGO.AddComponent<GraphicRaycaster>();

        // Creates a GameObject called "EventSystem"
        GameObject eventSystemGO = new GameObject("EventSystem");
        // Adds EventSystem component to EventSystem GameObject
        eventSystemGO.AddComponent<EventSystem>();
        // Adds StandaloneInputModule component to EventSystem GameObject
        eventSystemGO.AddComponent<StandaloneInputModule>();

        // Registers in the Undo system the creation of Canvas and EventSystem GameObjects
        // NOTE1: Registered Undo operations are grouped by click events.
        Undo.RegisterCreatedObjectUndo(canvasGO, "Create Hologram Visualizer");
        Undo.RegisterCreatedObjectUndo(eventSystemGO, "Create Hologram Visualizer");
    }
}
