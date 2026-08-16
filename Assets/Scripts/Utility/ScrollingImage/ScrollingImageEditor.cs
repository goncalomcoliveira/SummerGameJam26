#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ScrollingImage))]
public class ScrollingImageEditor : RawImageEditor {
    
    SerializedProperty scrollSpeed;

    protected override void OnEnable() {
        base.OnEnable();
        scrollSpeed = serializedObject.FindProperty("scrollSpeed");
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(scrollSpeed);
        serializedObject.ApplyModifiedProperties();
    }
}

#endif