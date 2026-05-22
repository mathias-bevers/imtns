using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CleanRoom.Utils
{
    [Serializable]
    public class SerializablePair<T, U>
    {
        [field: SerializeField] public T First {get; set; }
        [field: SerializeField] public U Second {get; set; }
    }

    public class SerializablePairEditor<T, U> : Editor
    {
        private SerializedProperty first;
        private SerializedProperty second;
        
        private void OnEnable()
        {
            serializedObject.FindProperty("First".ToBackingField());
            serializedObject.FindProperty("Second".ToBackingField());
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.PropertyField(first);
            EditorGUILayout.PropertyField(second);

            EditorGUILayout.EndHorizontal();
        }
    }


    [CustomEditor(typeof(SerializablePair<UnityEvent, float>))]
    public class SerializableEventFloatEditor : Editor
    {
        private SerializedProperty first;
        private SerializedProperty second;
        
        private void OnEnable()
        {
            serializedObject.FindProperty("First".ToBackingField());
            serializedObject.FindProperty("Second".ToBackingField());
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.PropertyField(first);
            EditorGUILayout.PropertyField(second);

            EditorGUILayout.EndHorizontal();
        }
    }
}