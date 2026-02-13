using UnityEngine;

namespace CleanRoom.Movement
{
    public abstract class MovementInput : MonoBehaviour
    {
        public abstract Vector2 GetInput();
    }
}