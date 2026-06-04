using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KattenKasteel.FSM.Editor
{
    [CustomEditor(typeof(Transitioner))]
    public class TransitionerEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = visualTreeAsset.CloneTree();
            return root;
        }
    }
}