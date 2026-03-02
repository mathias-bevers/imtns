using UnityEditor;
using UnityEngine;

namespace CleanRoom
{
    [CustomEditor(typeof(EnvironmentSpawner))]
    public class EnvironmentSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EnvironmentSpawner spawner = (EnvironmentSpawner)target;

            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Spawn"))
            {
                spawner.Spawn();
            }

            if (GUILayout.Button("Clear"))
            {
                spawner.Clear();
            }
            
            GUILayout.EndHorizontal();
        }
    }
}