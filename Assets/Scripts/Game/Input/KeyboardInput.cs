using UnityEngine;

namespace Game.Input
{
    public class KeyboardInput : IInputProvider
    {
        public float GetRotation()
        {
            return UnityEngine.Input.GetAxis("Horizontal");
        }

        public float GetMovement()
        {
            return UnityEngine.Input.GetAxis("Vertical");
        }

        public bool ActionButtonPressed()
        {
            return UnityEngine.Input.GetKey(KeyCode.Space);
        }
    }
}