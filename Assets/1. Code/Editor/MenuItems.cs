using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CleanRoom
{
    public static class MenuItems
    {
        [MenuItem("GameObject/CleanRoom/UI/MenuCanvas", false, 0)]
        private static void CreateMenuCanvas(MenuCommand command)
        {
            // set up canvas
            if (Object.Instantiate(Resources.Load("MenuCanvas")) is not GameObject menu)
            {
                throw new TypeLoadException("Could not load Prefab \"MenuCanvas\"");
            }

            menu.name = "MenuCanvas";
            GameObjectUtility.SetParentAndAlign(menu, command.context as GameObject);
            Undo.RegisterCreatedObjectUndo(menu, $"Create: ${menu.name}");
            Selection.SetActiveObjectWithContext(menu, null);
        }

        [MenuItem("Tools/CleanRoom/Force Recompile")]
        private static void ForceRecompile() => AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }
}