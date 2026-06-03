using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace KattenKasteel.FSM
{
    [System.Serializable]
    public class Transition 
    {
        [field: SerializeField] public State Target { get; private set; }
        [SerializeField] private Condition[] conditions;

        public bool CanTransition() => conditions.All(condition => condition.IsSatisfied(this));
    }
}