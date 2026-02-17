using System;
using CleanRoom.StateMachine;
using UnityEngine;

namespace CleanRoom.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour, IGameStateObject
    {
        [SerializeField] private MovementInput input;
        [SerializeField] private float movementSpeed;
        
        private new Rigidbody2D rigidbody2D = null;

        private void Start()
        {
            if (ReferenceEquals(null, input))
            {
                throw new Exception("the input has not been assigned");
            }

            rigidbody2D = GetComponent<Rigidbody2D>();
            transform.FindComponentUp<GameState>().AddStateObject(this);
        }

        private void Move(Vector2 inputAxis)
        {
            Vector2 target = rigidbody2D.position + (inputAxis * (Time.fixedDeltaTime * movementSpeed)); 
            rigidbody2D.MovePosition(target);
        }

        public void Tick(float deltaTime)
        {
            
        }
        
        public void FixedTick(float fixedDeltaTime)
        {
            Move(input.GetInput().normalized);
        }
    }
}