using UnityEngine;

namespace KattenKasteel.FSM
{
    public abstract class Condition : ScriptableObject
    {
        [field: SerializeField, TextArea] public string ErrorMessage { get; protected set; } = "condition not met";

        public abstract bool IsSatisfied(Transition transition);
    }
}