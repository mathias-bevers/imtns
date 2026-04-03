using System.Collections.Generic;
using UnityEngine;

namespace CleanRoom.StateMachine
{
    /// <summary>
    ///     This is an abstract class that serves as a base for <see cref="GameState" />
    ///     and <see cref="RoomState" /> It handles all default state behavior like
    ///     entering, exiting, keeping track of its <see cref="IGameStateObject" />s
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        /// <summary>
        ///     Indicates whether the state is currently active.
        /// </summary>
        public bool IsActive { get; private set; } = false;
        /// <summary>
        ///     Indicates if the state is completed.
        /// </summary>
        public bool IsCompleted { get; protected set; } = false;

        /// <summary>
        ///     A collection of <see cref="IGameStateObject" />s that are updated in the
        ///     <see cref="Tick" /> and <see cref="FixedTick" /> methods.
        /// </summary>
        private readonly List<IGameStateObject> gameStateObjects = new();

        /// <summary>
        ///     A cached version of <see cref="MonoBehaviour" />'s <c>tranfrom</c> field.
        /// </summary>
        private Transform cachedTransform;

        /// <summary>
        ///     This method is called when the <see cref="State" /> is registered to the
        ///     <see cref="StateMachine" />. It contains the logic that is needed to set
        ///     up the <see cref="State" /> into a working state.
        /// </summary>
        public virtual void Initialize()
        {
            cachedTransform = transform;
        }

        /// <summary>
        ///     This method is called by the <see cref="StateMachine" /> when the
        ///     <see cref="State" /> is entered. When the <paramref name="skipOnEnter" />
        ///     is false it also calls the  <see cref="OnEnter" /> function.
        /// </summary>
        /// <param name="skipOnEnter">
        ///     When true, the <see cref="OnEnter" /> is not called.
        /// </param>
        public void Enter(bool skipOnEnter = false)
        {
            for (int i = 0; i < cachedTransform.childCount; ++i)
            {
                cachedTransform.GetChild(i).gameObject.SetActive(true);
            }

            IsActive = true;

            if (skipOnEnter)
            {
                return;
            }

            OnEnter();
        }

        /// <summary>
        ///     A virtual method for implementing logic that needs to be executed when the
        ///     <see cref="State" /> is entered.
        /// </summary>
        protected virtual void OnEnter() { }

        /// <summary>
        ///     This method is used for adding <see cref="IGameStateObject" />s to the
        ///     <see cref="gameStateObjects" /> list. It also prevents duplicates in the
        ///     list.
        /// </summary>
        /// <param name="gameStateObject">
        ///     The <see cref="IGameStateObject" /> to add.
        /// </param>
        public void AddStateObject(IGameStateObject gameStateObject)
        {
            if (gameStateObjects.Contains(gameStateObject))
            {
                return;
            }

            gameStateObjects.Add(gameStateObject);
        }

        /// <summary>
        ///     This method removes a <see cref="IGameStateObject" /> from the
        ///     <see cref="gameStateObjects" /> list.
        /// </summary>
        /// <param name="gameStateObject">
        ///     The <see cref="IGameStateObject" /> to remove.
        /// </param>
        public void RemoveStateObject(IGameStateObject gameStateObject) =>
            gameStateObjects.Remove(gameStateObject);

        /// <summary>
        ///     This method is called every <c>Update</c> by the
        ///     <see cref="StateMachine" />, it passes a float containing
        ///     <see cref="Time.deltaTime" />. It calls all the
        ///     <see cref="IGameStateObject.Tick" /> methods in reverse order to prevent
        ///     errors when removing an object.
        /// </summary>
        /// <param name="deltaTime">Contains <see cref="Time.deltaTime" />.</param>
        public virtual void Tick(float deltaTime)
        {
            for (int i = gameStateObjects.Count - 1; i >= 0; --i)
            {
                if (ReferenceEquals(null, gameStateObjects[i]))
                {
                    continue;
                }

                gameStateObjects[i].Tick(deltaTime);
            }
        }

        /// <summary>
        ///     This method is called every <c>FixedUpdate</c> by the
        ///     <see cref="StateMachine" />, it passes a float containing
        ///     <see cref="Time.fixedDeltaTime" />. It calls all the
        ///     <see cref="IGameStateObject.FixedTick" /> methods in reverse order to
        ///     prevent errors when removing an object.
        /// </summary>
        /// <param name="fixedDeltaTime">
        ///     Contains <see cref="Time.fixedDeltaTime" />.
        /// </param>
        public virtual void FixedTick(float fixedDeltaTime)
        {
            for (int i = gameStateObjects.Count - 1; i >= 0; --i)
            {
                if (ReferenceEquals(null, gameStateObjects[i]))
                {
                    continue;
                }

                gameStateObjects[i].FixedTick(fixedDeltaTime);
            }
        }

        /// <summary>
        ///     This method is called by the <see cref="StateMachine" /> when the
        ///     <see cref="State" /> is exited.
        ///     When the <paramref name="skipOnExit" /> is false it also calls the
        ///     <see cref="OnExit" /> function.
        /// </summary>
        /// <param name="skipOnExit">
        ///     When true, the <see cref="OnExit" /> is not called.
        /// </param>
        public void Exit(bool skipOnExit = false)
        {
            IsActive = false;

            cachedTransform ??= transform;

            for (int i = 0; i < cachedTransform.childCount; ++i)
            {
                cachedTransform.GetChild(i).gameObject.SetActive(false);
            }

            if (skipOnExit)
            {
                return;
            }

            OnExit();
        }

        /// <summary>
        ///     A virtual method for implementing logic that needs to be executed when the
        ///     <see cref="State" /> is exited.
        /// </summary>
        protected virtual void OnExit() { }
    }
}