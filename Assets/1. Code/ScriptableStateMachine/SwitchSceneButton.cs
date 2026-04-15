using System;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.ScriptableStateMachine
{
    public class SwitchSceneButton : Button
    {
        public void SwitchState(ScriptableState state)
        {
            bool succes = StateMachine.Instance.TryEnterState(state);

            if (succes)
            {
                return;
            }

            throw new Exception("could not switch state.");
        }
    }
}