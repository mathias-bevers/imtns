using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CleanRoom.Editor
{
    public class SearchableSceneMenu : ScriptableObject, ISearchWindowProvider
    {
        private static readonly string PROJECT_SCENES_PATH = System.IO.Path.Combine("Assets", "2. Scenes");
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new();
            SearchTreeGroupEntry group = new(new GUIContent("Scene Assets"));
            tree.Add(group);
            
            string[] guids = AssetDatabase.FindAssets("t:SceneAsset");
            for (int i = 0; i < guids.Length; ++i)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(guids[i]);
                
                if (!scenePath.StartsWith(PROJECT_SCENES_PATH))
                {
                    continue;
                }
                
                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                SearchTreeEntry entry = new(new GUIContent(sceneAsset.name));
                entry.level = 1;
                entry.userData = sceneAsset;
                tree.Add(entry);
            }

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
    }
}