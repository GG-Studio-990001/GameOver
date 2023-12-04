using Codice.CM.Common;
using log4net.Util;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        public Rigidbody2D rigid;
        public Vector2 direction { get; private set; }
        public Vector2 nextDirection { get; private set; }
        public Vector3 startPosition { get; private set; }

        public float speed = 8f;
        public float speedMultiplier = 1f; // 팩맘의 청소기 습득 시 속도 변화를 위한 변수
        public Vector2 initialDirection;

        public LayerMask obstacleLayer { get; private set; }

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            startPosition = transform.position;
            obstacleLayer = LayerMask.GetMask("Obstacle");
        }

        private void Update()
        {
            if (nextDirection != Vector2.zero)
            {
                SetDirection(nextDirection);
            }
        }

        public void Move()
        {
            // 길 너비와 몸 지름이 같기 때문에 rigidbody를 주체로 움직임
            Vector2 position = rigid.position;
            Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

            rigid.MovePosition(position + translation);
        }

        public void ResetState()
        {
            direction = initialDirection;
            nextDirection = Vector2.zero;
            transform.position = startPosition;
        }

        public void SetNextDirection(Vector2 direction)
        {
            nextDirection = direction;
        }

        public void SetDirection(Vector2 direction)
        {
            if (!CheckRoadBlocked(direction))
            {
                this.direction = direction;
                nextDirection = Vector2.zero;
            }
        }

        public bool CheckRoadBlocked(Vector2 direction)
        {
            // 방향에 길이 막혀있으면 true 반환
            // 몸집이 있기 때문에 box로 검출
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1.0f, obstacleLayer);

            return hit.collider != null;
        }
    }
}