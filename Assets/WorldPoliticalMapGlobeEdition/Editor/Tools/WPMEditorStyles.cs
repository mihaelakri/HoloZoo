﻿using UnityEngine;
using UnityEditor;

namespace WPM {

    public static class WPMEditorStyles {

        public static void SetFoldoutColor(this GUIStyle style) {
            Color foldoutColor = EditorGUIUtility.isProSkin ? new Color(0.52f, 0.66f, 0.9f) : new Color(0.12f, 0.16f, 0.4f);
            style.normal.textColor = foldoutColor;
            style.onNormal.textColor = foldoutColor;
            style.hover.textColor = foldoutColor;
            style.onHover.textColor = foldoutColor;
            style.focused.textColor = foldoutColor;
            style.onFocused.textColor = foldoutColor;
            style.active.textColor = foldoutColor;
            style.onActive.textColor = foldoutColor;
            style.fontStyle = FontStyle.Bold;
        }

        public static Color HDRColorPicker(string label, Color value, bool showAlpha = true) {
            return EditorGUILayout.ColorField(new GUIContent(label), value, true, showAlpha, true);
        }
    }
}


