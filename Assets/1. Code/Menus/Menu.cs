using System;
using UnityEngine;

namespace CleanRoom.Menus
{
    public abstract class Menu : MonoBehaviour
    {
        [field: SerializeField] public bool IsHUD { get; protected set; }

        public bool IsOpen { get; private set; }

        public event Action openedEvent;
        public event Action closedEvent;
    }
}