using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KattenKasteel.FSM.Editor
{
    [CustomPropertyDrawer(typeof(Transition))]
    public class TransitionDrawer : PropertyDrawer
    {
        [SerializeField] public VisualTreeAsset visualTreeAsset;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            TemplateContainer root = visualTreeAsset.CloneTree();
            return root;
        }
    }
}