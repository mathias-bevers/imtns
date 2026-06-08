using System;

namespace KattenKasteel.FSM
{
    public class ConditionNotSatisfiedException : Exception
    {
        public ConditionNotSatisfiedException(string message) : base(message) { }
    }
}