namespace Game.Input
{
    public class MobileInput : IInputProvider
    {
        public float GetRotation()
        {
            return SimpleInput.GetAxis("Horizontal");
        }

        public float GetMovement()
        {
            return SimpleInput.GetAxis("Vertical");
        }

        public bool ActionButtonPressed()
        {
            return SimpleInput.GetButton("Action");
        }
    }
}