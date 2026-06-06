using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CleanRoom.Editor
{
    public class SearchableSceneMenu : ScriptableObject, ISearchWindowProvider
    {
        private static readonly string PROJECT_SCENES_PATH = Path.Combine("Assets", "2. Scenes");
        private static readonly int PROJECT_SCENE_DELIMITER_COUNT =
            PROJECT_SCENES_PATH.Count(c => c == Path.DirectorySeparatorChar);

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new();
            SearchTreeGroupEntry rootGroup = new(new GUIContent("Scene Assets"));
            tree.Add(rootGroup);

            string[] scenePaths = AssetDatabase.FindAssets("t:SceneAsset")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(p => p.StartsWith(PROJECT_SCENES_PATH))
                .ToArray();
            
            Array.Sort(scenePaths);

            for (int i = 0; i < scenePaths.Length; ++i)
            {
                string scenePath = scenePaths[i];
                AddEntryForPath(ref tree, scenePath);
            }

            string log = "TREE:\n";

            foreach (SearchTreeEntry searchTreeEntry in tree)
            {
                log = string.Concat(log, '\n', searchTreeEntry.name, " at level: \t", searchTreeEntry.level);
            }

            Debug.Log(log);

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (searchTreeEntry.userData is not SceneAsset sceneAsset)
            {
                throw new InvalidOperationException("Can only open scene assets not: " +
                                                    searchTreeEntry.userData.GetType().Name);
            }

            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneAsset));
            return true;
        }

        private static void AddEntryForPath(ref List<SearchTreeEntry> tree, string fullPath)
        {
            string relativePath = fullPath.Remove(0, PROJECT_SCENES_PATH.Length + 1);
            int delimiterIndex = relativePath.IndexOf(Path.DirectorySeparatorChar);

            while(delimiterIndex > 0)
            {
                string group = relativePath[..delimiterIndex];
                
                if (!tree.Any(entry => entry is SearchTreeGroupEntry && entry.name == group))
                {
                    int level = relativePath.Count(c => c == Path.DirectorySeparatorChar);
                    tree.Add(new SearchTreeGroupEntry(new GUIContent(group), level));
                }
                
                relativePath = relativePath[(delimiterIndex + 1)..];
                delimiterIndex = relativePath.IndexOf(Path.DirectorySeparatorChar);
            } 
            
            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(fullPath);
            SearchTreeEntry entry = new(new GUIContent(sceneAsset.name));
            entry.level = fullPath.Count(c => c == Path.DirectorySeparatorChar) - PROJECT_SCENE_DELIMITER_COUNT;
            entry.userData = sceneAsset;
            tree.Add(entry);
        }
    }
}