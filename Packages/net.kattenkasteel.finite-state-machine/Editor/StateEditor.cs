using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KattenKasteel.FSM.Editor
{
    [CustomEditor(typeof(State))]
    public class StateEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = visualTreeAsset.CloneTree();
            VisualElement parentSettings = root.Q<VisualElement>("ParentSettings");
            Toggle isParent = root.Q<Toggle>("IsParent");
            
            // enable/disable the parent settings when the toggle is changed
            isParent.RegisterCallback<ChangeEvent<bool>>(changeEvent =>
            {
                parentSettings.style.display = changeEvent.newValue ? DisplayStyle.Flex : DisplayStyle.None;
            });
            
            
            return root;
        }
    }
}