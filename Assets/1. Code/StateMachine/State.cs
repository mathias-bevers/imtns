using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace CleanRoom.StateMachine
{
    /// <summary>
    ///     This is an abstract class that serves as a base for <see cref="State" />
    ///     and <see cref="RoomState" /> It handles all default state behavior like
    ///     entering, exiting.
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        /// <summary>
        ///     Indicated if the state can be entered.
        /// </summary>
        public virtual bool CanEnter => !StateMachine.Instance.IsStateCompleted(GetType().Name);

        /// <summary>
        ///     Indicated if the state can be exited.
        /// </summary>
        public virtual bool CanExit { get; protected set; } = true;
        
        /// <summary>
        ///     Invoked when the state is completed.
        /// </summary>
        public event Action<string> completedEvent;

        [field: SerializeField] public string StateName { get; protected set; }
        [field: SerializeField, Scene] public string SceneName { get; protected set; }
        [field: SerializeField] public UnityEvent EnterEvent { get; private set; }
        [field: SerializeField] public UnityEvent ExitEvent { get; private set; }


        public virtual void Enter()
        {
            EnterEvent?.Invoke();
        }

        public void Exit()
        {
            ExitEvent?.Invoke();
        }

        public virtual void Complete(string name = null)
        {
            completedEvent?.Invoke(string.IsNullOrEmpty(name) ? StateName : name);
        }
    }
}