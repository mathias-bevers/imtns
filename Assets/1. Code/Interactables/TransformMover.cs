using System;
using KattenKasteel.FSM;
using UnityEngine;

namespace CleanRoom.Interactables
{
    public class TransformMover : MonoBehaviour
    {
        [SerializeField] private Transitioner transitioner;
        [SerializeField] private Transform movee;
        [SerializeField] private Transform target;

        public void MoveTransform()
        {
            if (!ReferenceEquals(null, transitioner) && !transitioner.CanTransition())
            {
                return;
            }
            
            movee.position = target.position;
        }
    }
}