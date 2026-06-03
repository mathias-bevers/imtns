using KattenKasteel.FMS.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KattenKasteel.FSM.Editor
{
    [CustomEditor(typeof(Condition), true)]
    public class ConditionEditor : UnityEditor.Editor
    {
        private static readonly StyleLength PROPERTY_FONT_SIZE = new(16);
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = visualTreeAsset.CloneTree();
            VisualElement container = root.Q<VisualElement>("PropertyContainer");

            SerializedProperty it = serializedObject.FindProperty("ErrorMessage".ToBackingField()).Copy();
            while (it.Next(false))
            {
                PropertyField field = new(it);
                field.style.fontSize = PROPERTY_FONT_SIZE;
                container.Add(field);
            }

            return root;
        }
    }
}