using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.CH1.Pacmom
{
    public class Rapley : MonoBehaviour
    {
        public Movement movement;

        private void Start()
        {
            movement.spriteRotation.canRotate = false;
            movement.spriteRotation.canFlip = true;

            ResetState();
        }

        public void ResetState()
        {
            movement.ResetState();
        }

        private void FixedUpdate()
        {
            movement.Move();
        }

        private void OnMove(InputValue value)
        {
            Vector2 inputDirection = value.Get<Vector2>();
            if (inputDirection != Vector2.zero)
            {
                movement.SetNextDirection(inputDirection);
            }
        }
    }
}