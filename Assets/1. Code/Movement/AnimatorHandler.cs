using System;
using UnityEngine;

namespace CleanRoom.Movement
{
    public class AnimatorHandler : MonoBehaviour
    {
        private static readonly int MOVE_X = Animator.StringToHash("MoveX");
        private static readonly int MOVE_Y = Animator.StringToHash("MoveY");
        private static readonly int LAST_MOVE_X = Animator.StringToHash("LastMoveX");
        private static readonly int LAST_MOVE_Y = Animator.StringToHash("LastMoveY");
        private static readonly int MOVEMENT_MAGNITUDE = Animator.StringToHash("MoveMagnitude");
        [SerializeField] private Animator animator;
        
        private MovementInput moveHandler;
        private Vector2 input;
        private Vector2 lastMoveDirection;
        
        private void Awake()
        {
            moveHandler = GetComponent<Movement>().Input;
        }

        private void Update()
        {
            Vector2 newInput = moveHandler.GetInput();
            
            if (newInput.magnitude == 0 && input.magnitude != 0)
            {
                lastMoveDirection = input;
            }

            input = newInput;
        }

        private void LateUpdate()
        {
            Animate();
        }

        private void Animate()
        {
            animator.SetFloat(MOVE_X, input.x);
            animator.SetFloat(MOVE_Y, input.y);
            
            animator.SetFloat(MOVEMENT_MAGNITUDE, input.magnitude);
            
            animator.SetFloat(LAST_MOVE_X, lastMoveDirection.x);
            animator.SetFloat(LAST_MOVE_Y, lastMoveDirection.y);
        }
    }
}