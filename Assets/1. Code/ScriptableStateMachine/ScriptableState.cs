using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CleanRoom.ScriptableStateMachine
{
    /// <summary>
    ///     This is an abstract class that serves as a base for <see cref="ScriptableState" />
    ///     and <see cref="ScriptableRoomState" /> It handles all default state behavior like
    ///     entering, exiting.
    /// </summary>
    public abstract class ScriptableState : ScriptableObject
    {
        /// <summary>
        ///     Indicates if the state is completed.
        /// </summary>
        public bool IsCompleted { get; protected set; } = false;

        /// <summary>
        ///     Indicated if the state can be entered.
        /// </summary>
        public bool CanEnter { get; protected set; } = true;

        /// <summary>
        ///     Indicated if the state can be exited.
        /// </summary>
        public bool CanExit { get; protected set; } = true;

        [field: SerializeField, Scene] public string SceneName { get; protected set; }

        [field: SerializeField] public UnityEvent EnterEvent { get; protected set; }

        [field: SerializeField] public UnityEvent ExitEvent { get; protected set; }
        
        public void Enter()
        {
            EnterEvent?.Invoke();
        }

        public virtual void Exit()
        {
            
            ExitEvent.Invoke();
        }
    }
}