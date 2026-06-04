using System;
using UnityEngine;

namespace KattenKasteel.FSM
{
    [Serializable]
    public class Transition 
    {
        [field: SerializeField] public State Target { get; internal set; }
        [SerializeField] private Condition[] conditions;

        public bool CanTransition(out string conditionMessages)
        {
            conditionMessages = string.Empty;
            
            if (ReferenceEquals(null, conditions) || conditions.Length == 0)
            {
                return true;
            }

            bool canTransition = true;
            foreach (Condition condition in conditions)
            {
                if (condition.IsSatisfied(this))
                {
                    continue;
                }

                canTransition = false;
                conditionMessages = string.Concat(conditionMessages, condition.ErrorMessage, Environment.NewLine);
            }
            
            return canTransition;
        }
    }
}