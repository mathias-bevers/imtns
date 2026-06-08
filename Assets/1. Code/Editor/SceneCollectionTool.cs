using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Toolbars;
using UnityEngine;

namespace CleanRoom.Editor
{
    // source:  https://www.youtube.com/watch?v=XFIkkDDFHfs
    public static class SceneCollectionTool
    {
        private const string SCENE_ICON_NAME = "SceneAsset Icon";
        private const string SCENE_TOOLTIP = "Opens scene selection tool.";

        [MainToolbarElement("Scene Button", defaultDockPosition = MainToolbarDockPosition.Right)]
        public static MainToolbarElement GetScenesButton()
        {
            Texture2D assetIcon = EditorGUIUtility.IconContent(SCENE_ICON_NAME).image as Texture2D;
            return new MainToolbarButton(new MainToolbarContent("Scene", assetIcon, SCENE_TOOLTIP),
                OnOpenSearchableMenu);
        }

        private static void OnOpenSearchableMenu()
        {
            SearchWindowContext context = new(GUIUtility.GUIToScreenPoint(Event.current.mousePosition));
            SearchableSceneMenu menu = ScriptableObject.CreateInstance<SearchableSceneMenu>();
            SearchWindow.Open(context, menu);
        }
    }
}