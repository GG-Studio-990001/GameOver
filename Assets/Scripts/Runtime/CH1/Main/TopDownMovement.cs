using UnityEngine;

namespace Runtime.CH1.Main
{
    public class TopDownMovement
    {
        private readonly Transform _transform;
        private readonly float _moveSpeed = 5.0f;
        private Vector2 _previousMovementInput;
        
        public TopDownMovement(float moveSpeed, Transform transform)
        {
            _moveSpeed = moveSpeed;
            _transform = transform;
        }
        
        public void Move(Vector2 movementInput)
        {
            if (movementInput == Vector2.zero)
            {
                return;
            }
            
            if (movementInput.magnitude > 1.0f)
            {
                movementInput = _previousMovementInput;
            }
            
            Vector2 movement = movementInput * (_moveSpeed * Time.deltaTime);
            
            _transform.Translate(movement);
            
            _previousMovementInput = movementInput;
        }
    }
}