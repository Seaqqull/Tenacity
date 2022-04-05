using Tenacity.Base;
using UnityEngine;
using Pathfinding;


namespace Tenacity.Player
{
    public class PlayerMovement : BaseMono
    {
        [SerializeField] private Animator _animator;

        private AIPath _ai;


        protected override void Awake()
        {
            base.Awake();
  
            _ai = GetComponent<AIPath>();
        }

        protected void LateUpdate()
        {
            var movementDirection = Quaternion.Inverse(Transform.rotation) * _ai.desiredVelocity;
            var movementMagnitude = movementDirection.magnitude;
            
            _animator.SetBool(Utility.Constants.Animation.WALKING, (movementMagnitude > 0.0f));
            _animator.SetBool(Utility.Constants.Animation.RUNNING, (movementMagnitude > 0.5f));
            _animator.SetFloat(Utility.Constants.Animation.DIRECTION, movementDirection.z);
            _animator.SetFloat(Utility.Constants.Animation.SIDE, movementDirection.x);
        }
    }
}