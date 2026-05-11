using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace CleanRoom.StateMachine.Editor
{
    [CustomEditor(typeof(RoomState))]
    public class RoomStateEditor : UnityEditor.Editor
    {
        private List<SerializedProperty> properties = new();

        private void OnEnable()
        {
           properties.Clear();
           properties.Add(serializedObject.FindProperty(ToBackingField("StateName")));
           properties.Add(serializedObject.FindProperty(ToBackingField("SceneName")));
           properties.Add(serializedObject.FindProperty(ToBackingField("NextRoom")));
           properties.Add(serializedObject.FindProperty(ToBackingField("EnterEvent")));
           properties.Add(serializedObject.FindProperty(ToBackingField("ExitEvent")));
           properties.Add(serializedObject.FindProperty(ToBackingField("UnfocusEvent")));
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

        private static string ToBackingField(string source)
        {
            return string.Concat('<', source, '>', "k__BackingField");
        }
    }
}