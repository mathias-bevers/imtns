using System;
using UnityEngine;

namespace KattenKasteel.FSM.Conditions
{
    [CreateAssetMenu(fileName = "State Completion", menuName = "KattenKasteel/FSM/Conditions/State Completion")]
    public class StateCompletionCondition : Condition
    {
        [SerializeField] private CheckingType checkingType;
        [SerializeField] private bool isCompleted;

        public override bool IsSatisfied(Transition transition)
        {
            if (checkingType == CheckingType.TargetState && (ReferenceEquals(transition?.Target, null)))
            {
                throw new ArgumentNullException(nameof(transition), "cannot check target if transition/target is null");
            }
            
            bool result = checkingType switch
            {
                CheckingType.ActiveState => StateMachine.Instance.ActiveState.IsCompleted == isCompleted,
                CheckingType.TargetState => transition.Target.IsCompleted == isCompleted,
                _ => throw new ArgumentOutOfRangeException("cannot process for checking type: " + checkingType)
            };

            return result;
        }

        private enum CheckingType { ActiveState, TargetState }
    }
}