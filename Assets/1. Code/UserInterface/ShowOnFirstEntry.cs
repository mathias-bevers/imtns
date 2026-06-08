using System;
using KattenKasteel.FSM;
using UnityEngine;

namespace CleanRoom.UserInterface
{
    public class ShowOnFirstEntry : MonoBehaviour
    {
        private void OnEnable()
        {
            bool hasBeenVisited = StateMachine.Instance.ActiveState.HasBeenVisited;
            if (hasBeenVisited)
            {
                gameObject.SetActive(false);
            }
        }
    }
}