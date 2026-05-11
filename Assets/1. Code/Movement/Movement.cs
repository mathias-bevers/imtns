using System;
using UnityEngine;

namespace CleanRoom.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        [field: SerializeField] public MovementInput Input { get; private set; }
        [SerializeField] private float movementSpeed;
        
        private new Rigidbody2D rigidbody2D = null;

        private void Start()
        {
            if (ReferenceEquals(null, Input))
            {
                throw new Exception("the input has not been assigned");
            }

            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Move(Vector2 inputAxis)
        {
            Vector2 target = rigidbody2D.position + (inputAxis * (Time.fixedDeltaTime * movementSpeed)); 
            rigidbody2D.MovePosition(target);
        }
        
        public void FixedUpdate()
        {
            Move(Input.GetInput());
        }
    }
}