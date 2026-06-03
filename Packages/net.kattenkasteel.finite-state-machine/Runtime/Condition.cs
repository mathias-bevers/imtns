using UnityEngine;

namespace KattenKasteel.FSM
{
    public abstract class Condition : ScriptableObject
    {
        [field: SerializeField, TextArea] protected string ErrorMessage { get; private set; } = "condition not met";

        public abstract bool IsSatisfied(Transition transition);
    }
}