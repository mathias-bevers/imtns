using CleanRoom.Interactables;
using UnityEngine;

namespace CleanRoom
{
    public class Player : Singleton<Player>
    {
        [field: SerializeField] public InteractionButton interactionButton { get; private set; }
    }
}