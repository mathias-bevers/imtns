using KattenKasteel.FSM;
using UnityEngine;

namespace CleanRoom
{
    public class Player : Singleton<Player>
    {
        private const string DRESSING_GAME_STATE_NAME = "Aankleed Procedure";
        private static readonly int IS_GOWNED = Animator.StringToHash("IsGowned");

        [SerializeField] private Animator animator;

        private State dressingState;

        public override void Awake()
        {
            base.Awake();
            dressingState = StateMachine.Instance.GetState(s => string.Equals(DRESSING_GAME_STATE_NAME, s.StateName));
            animator.SetBool(IS_GOWNED, dressingState.IsCompleted); //TODO: uncomment when done testing
        }
    }
}