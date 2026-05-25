using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using CleanRoom.Utils;

namespace CleanRoom.StateMachine.Editor
{
    [CustomEditor(typeof(RoomState))]
    public class RoomStateEditor : UnityEditor.Editor
    {
        private List<SerializedProperty> properties = new();

        private void OnEnable()
        {
           properties.Clear();
           properties.Add(serializedObject.FindProperty("gameStates"));
           properties.Add(serializedObject.FindProperty("StateName".ToBackingField()));
           properties.Add(serializedObject.FindProperty("SceneName".ToBackingField()));
           properties.Add(serializedObject.FindProperty("NextRoom".ToBackingField()));
           properties.Add(serializedObject.FindProperty("EnterEvent".ToBackingField()));
           properties.Add(serializedObject.FindProperty("ExitEvent".ToBackingField()));
           properties.Add(serializedObject.FindProperty("UnfocusEvent".ToBackingField()));
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            for (int i = 0; i < properties.Count; ++i)
            {
                EditorGUILayout.PropertyField(properties[i]);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}