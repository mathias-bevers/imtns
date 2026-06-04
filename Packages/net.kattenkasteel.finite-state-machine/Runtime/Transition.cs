using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace KattenKasteel.FSM
{
    [System.Serializable]
    public class Transition 
    {
        [field: SerializeField] public State Target { get; internal set; }
        [SerializeField] private Condition[] conditions;

        public bool CanTransition()
        {
            if (ReferenceEquals(null, conditions) || conditions.Length == 0)
            {
                return true;
            }
            
            return conditions.All(condition => condition.IsSatisfied(this));
        }
    }
}