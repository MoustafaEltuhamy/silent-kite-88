using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;


namespace AGAPI.Gameplay.Editor
{
    [CustomEditor(typeof(StretchGridLayoutGroup))]
    [CanEditMultipleObjects]
    public class StretchGridLayoutGroupEditor : GridLayoutGroupEditor
    {
        SerializedProperty fitTypeProp;
        SerializedProperty initialCellSizeProp;
        SerializedProperty rowsProp;
        SerializedProperty columnsProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            // grab your serialized props
            fitTypeProp = serializedObject.FindProperty("fitType");
            initialCellSizeProp = serializedObject.FindProperty("initialCellSize");
            rowsProp = serializedObject.FindProperty("rows");
            columnsProp = serializedObject.FindProperty("columns");
        }

        public override void OnInspectorGUI()
        {
            // draw the base GridLayoutGroup fields first
            base.OnInspectorGUI();

            // then your extras
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Stretch Grid Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(fitTypeProp);
            EditorGUILayout.PropertyField(initialCellSizeProp);
            EditorGUILayout.PropertyField(rowsProp);
            EditorGUILayout.PropertyField(columnsProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
