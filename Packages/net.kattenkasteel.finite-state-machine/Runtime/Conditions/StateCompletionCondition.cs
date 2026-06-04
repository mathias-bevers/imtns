using System;
using UnityEngine;

namespace KattenKasteel.FSM.Conditions
{
    [CreateAssetMenu(fileName = "State Completion", menuName = "KattenKasteel/FSM/Conditions/State Completion")]
    public class StateCompletionCondition : Condition
    {
        private enum CheckingType { ActiveState, TargetState };

        [SerializeField] private CheckingType checkingType;
        [SerializeField] private bool isCompleted;
        
        public override bool IsSatisfied(Transition transition)
        {
            return checkingType switch
            {
                CheckingType.ActiveState => StateMachine.Instance.ActiveState == isCompleted,
                CheckingType.TargetState => transition.Target.IsCompleted == isCompleted,
                _ => throw new ArgumentOutOfRangeException("cannot process for checking type: " + checkingType)
            };
        }
    }
}