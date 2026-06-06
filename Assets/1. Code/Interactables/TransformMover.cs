using System;
using UnityEngine;

namespace CleanRoom.Interactables
{
    public class TransformMover : MonoBehaviour
    {
        [SerializeField] private Transform movee;
        [SerializeField] private Transform target;

        public void MoveTransform()
        {
            movee.position = target.position;
        }
    }
}