using CleanRoom.Interactables;
using CleanRoom.Movement;

namespace CleanRoom.Menus
{
    public class OnScreenControllerMenu : Menu
    {
        public OnScreenJoystick Joystick { get; private set; }
        public InteractionButton InteractionButton { get; private set; }

        private void Awake()
        {
            Joystick = GetComponentInChildren<OnScreenJoystick>(true);
            InteractionButton = GetComponentInChildren<InteractionButton>(true);
        }

        protected override bool CanBeOpened() => true;

        protected override bool CanBeClosed() => true;
    }
}