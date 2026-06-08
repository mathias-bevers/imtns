using UnityEngine;

namespace KattenKasteel.FSM.Conditions
{
    [CreateAssetMenu(fileName = "Always True", menuName = "KattenKasteel/FSM/Conditions/Always True")]
    public class AlwaysTrueCondition : Condition
    {
        public override bool IsSatisfied(Transition transition) => true;
    }
}
