using UnityEngine;

namespace CodeBase.Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        private const string Button = "Fire";

        public abstract Vector2 Axis { get; }
        public abstract float AxisY { get; }

        public bool IsAttackButtonUp() =>
            SimpleInput.GetButtonUp(Button);

        protected static float HorizontalAxis() =>
            SimpleInput.GetAxis(Horizontal);

        protected static Vector2 SimpleInputAxis() =>
            new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
}