using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace KattenKasteel.FSM
{
    [CreateAssetMenu(fileName = "State", menuName = "KattenKasteel/FSM/State")]
    public class State : ScriptableObject
    {
        [field: SerializeField] public string StateName { get; private set; }
        [field: SerializeField, Scene] public int SceneIndex { get; private set; }
        [field: SerializeField] public bool IsParent { get; private set; }
        [field: SerializeField, ShowIf("IsParent")]
        public State[] Children { get; private set; }

        public bool IsCompleted
        {
            get => IsParent
                ? Children.All(c => c.IsCompleted)
                : isCompleted;
            set => isCompleted = value;
        }

        private bool isCompleted = false;
    }
}