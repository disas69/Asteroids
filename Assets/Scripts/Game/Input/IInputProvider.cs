namespace Game.Input
{
    public interface IInputProvider
    {
        float GetRotation();
        float GetMovement();
        bool ActionButtonPressed();
    }
}