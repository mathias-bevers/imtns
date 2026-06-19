using System;
using CleanRoom.Utils;
using KattenKasteel.FSM;
using Newtonsoft.Json;
using UnityEngine;

namespace CleanRoom.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        [Serializable]
        private struct Vec2
        {
            public float x;
            public float y;
        }

        private const string SAVE_SUFFIX = "_player-position.json";

        [field: SerializeField] public MovementInput Input { get; private set; }
        [SerializeField] private float movementSpeed;

        private new Rigidbody2D rigidbody2D = null;
        private string filename = string.Empty;

        private void Start()
        {
            if (ReferenceEquals(null, Input))
            {
                throw new Exception("the input has not been assigned");
            }

            rigidbody2D = GetComponent<Rigidbody2D>();

            filename = StateMachine.Instance.ActiveState.StateName.ToLower().Replace(' ', '-');
            filename += SAVE_SUFFIX;

            if (!SaveSystem.Load(filename, out string contents))
            {
                return;
            }

            Vec2 vec2 = JsonConvert.DeserializeObject<Vec2>(contents);
            transform.position = new Vector3(vec2.x, vec2.y, 0);
        }

        private void OnDestroy()
        {
            Vec2 vec2 = new() { x = rigidbody2D.position.x, y = rigidbody2D.position.y };
            SaveSystem.Save(filename, JsonConvert.SerializeObject(vec2));
        }

        public void FixedUpdate()
        {
            Move(Input.GetInput());
        }

        private void Move(Vector2 inputAxis)
        {
            Vector2 target = rigidbody2D.position + inputAxis * (Time.fixedDeltaTime * movementSpeed);
            rigidbody2D.MovePosition(target);
        }
    }
}