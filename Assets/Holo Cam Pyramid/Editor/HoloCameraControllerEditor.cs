using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Adds a slider to HoloCameraController component for control the camera dist to center in edit mode.
/// </summary>
// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(HoloCameraController))]
[CanEditMultipleObjects]
public class HoloCameraControllerEditor : Editor {

    SerializedProperty distCamToCenterProp;

    // Slider min value
    private float minVal = 0.01f;

    // Slider max value
    private float maxVal = 100f;

    private void OnEnable()
    {
        // Fetchs the objects from the GameObject script to display in the inspector
        distCamToCenterProp = serializedObject.FindProperty("cameraToCenterDistance");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        EditorGUILayout.Slider(distCamToCenterProp, minVal, maxVal, new GUIContent("Dist to center"));
        HoloCameraController camCtrl = target as HoloCameraController;
        // Calls the method ChangeBoxSize from HoloCameraController for update in edit mode the zoom of PyramidCamera
        camCtrl.SetCamToCenterDistance( camCtrl.cameraToCenterDistance );

        // Applies changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }

}


/* HELPFUL DOCS FOR MAKE THIS SCRIPT
 * How to edit fields of components serializable objects
* https://docs.unity3d.com/ScriptReference/EditorGUILayout.PropertyField.html
* 
* Creating custom editors:
* https://docs.unity3d.com/ScriptReference/Editor.html
* 
* If you want the Editor to support multi-object editing, you can use the CanEditMultipleObjects attribute.
* Instead of modifying script variables directly,it's advantageous to use the SerializedObject and
* SerializedProperty system to edit them, since this automatically handles multi-object
* editing, undo, and prefab overrides. 
*/
