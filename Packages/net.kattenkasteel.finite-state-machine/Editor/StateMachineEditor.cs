using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace KattenKasteel.FSM.Editor
{
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = visualTreeAsset.CloneTree();
            return root;
        }
    }
}