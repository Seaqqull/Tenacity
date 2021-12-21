using Tenacity.Managers;
using UnityEngine;


namespace Tenacity.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Vector2 _velocityScale;
        [SerializeField][Range(0.0f, 1.0f)] private float _groundCheck;

        public bool BlockInput { get; set; }

        private CompositeCollider2D _collider;
        private bool _jumpProceeded;
        private Rigidbody2D _body;
        private Vector2 _amount;
        private bool _grounded;
        private bool _jump;


        private void Awake()
        {
            _collider = GetComponent<CompositeCollider2D>();
            _body = GetComponent<Rigidbody2D>();

            _jumpProceeded = true;
        }

        private void Update()
        {
            if (BlockInput) return;

            bool shiftPressed = InputManager.ShiftButtonPressed;
            float direction = InputManager.RightButtonPressed ? 1.0f : 
                InputManager.LeftButtonPressed ? -1.0f : 0.0f;

            _grounded = CheckGround();

            _amount = new Vector3(direction * _velocityScale.x * (shiftPressed? 1.0f : 0.5f), 0 * _velocityScale.y, 0);
            _jump = InputManager.SpaceButtonPressed;

            UpdateAnimation(direction, shiftPressed? 1.0f : 0.0f);
        }

        private void FixedUpdate()
        {
            if (BlockInput) return;

            if(!_jumpProceeded)
            {
                _jumpProceeded = true;
                _jump = false;

                _body.velocity = (Vector2.up * _velocityScale.y);
            }
            if(_grounded)
                _body.velocity = new Vector2(_amount.x, _body.velocity.y);
        }


        private bool CheckGround()
        {
            return (Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0.0f, Vector2.down, _groundCheck, _groundMask).collider != null)? true : false;
        }

        private void UpdateAnimation(float direction, float speed)
        {
            bool isRunning = (direction != 0);

            if(_jump && _grounded)
            {
                _jumpProceeded = false;
                _animator.SetBool(Utility.Constants.Animation.JUMP, _jump);
            }

            _animator.SetInteger(Utility.Constants.Animation.DIRECTION, (int)direction);
            _animator.SetBool(Utility.Constants.Animation.MOVE, isRunning);

            if(!_jump && _body.velocity.y == 0 && _grounded)
            {
                _animator.SetBool(Utility.Constants.Animation.FALLING, false);
                _animator.SetBool(Utility.Constants.Animation.JUMP, false);
            }
            else if(_body.velocity.y < 0)
            {
                _animator.SetBool(Utility.Constants.Animation.FALLING, true);
            }
        }


        public void OnDead()
        {
            BlockInput = true;
            _animator.SetBool(Utility.Constants.Animation.DEAD, true);
            _amount = Vector2.zero;
        }
    }
}