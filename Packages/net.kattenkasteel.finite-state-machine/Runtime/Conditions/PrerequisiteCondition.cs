using System.Linq;
using KattenKasteel.FSM;
using UnityEngine;

namespace CleanRoom.Utils
{
    [CreateAssetMenu(fileName = "Prerequisites", menuName = "KattenKasteel/FSM/Conditions/Prerequisites")]
    public class PrerequisiteCondition : Condition
    {
        [SerializeField] private State[] statesToComplete;
        
        public override bool IsSatisfied(Transition transition) => statesToComplete.All(state => state.IsCompleted);
    }
}