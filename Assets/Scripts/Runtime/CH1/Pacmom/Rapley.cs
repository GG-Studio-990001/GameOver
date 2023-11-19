using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rapley : MonoBehaviour
    {
        public Rigidbody2D rigid { get; private set; }
        public Vector2 direction { get; private set; }
        public Vector2 nextDirection { get; private set; }
        public Vector3 startingPosition { get; private set; }

        [Header("Speeds")]
        public float speed = 8f;
        public float speedMultiplier = 1f; // 팩맘의 청소기 습득 시 속도 변화를 위한 변수

        [Header("Else")]
        public LayerMask obstacleLayer;


        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            startingPosition = transform.position;
        }

        private void Start()
        {
            ResetState();
        }

        public void ResetState()
        {
            if (gameObject.activeInHierarchy == false)
            {
                gameObject.SetActive(true);
            }
            
            direction = new Vector2(1f, 0f); // initial direction 오른쪽
            nextDirection = Vector2.zero;
            transform.position = startingPosition;
        }

        private void Update()
        {
            if (nextDirection != Vector2.zero)
            {
                SetDirection(nextDirection);
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            // 길 너비와 몸 지름이 같기 때문에 rigidbody를 주체로 움직임
            Vector2 position = rigid.position;
            Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

            rigid.MovePosition(position + translation);
        }

        private void OnMove(InputValue value)
        {
            Vector2 inputDirection = value.Get<Vector2>();
            if (inputDirection != Vector2.zero)
            {
                nextDirection = inputDirection;
            }
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