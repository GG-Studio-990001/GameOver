using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.CH1.Pacmom
{
    public class Rapley : MonoBehaviour
    {
        public MovementAndRotation movement;

        private void Start()
        {
            SetSpriteRotation();
            ResetState();
        }

        private void SetSpriteRotation()
        {
            movement.spriteRotation.SetCanRotate(false);
            movement.spriteRotation.SetCanFlip(true);
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