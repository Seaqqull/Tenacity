using UnityEngine;
using System;


namespace Tenacity.Behaviour.Enemies
{
    public class SimpleEnemy : Enemy
    {
        protected Vector2 _movementPosition;
        protected float _currentSpeed;


        protected override bool MovementNeeded()
        {
            var horizontalMovement = Mathf.Abs(_movementDirection.x);

            return (Math.Round(horizontalMovement, 2) > 0.1) &&
                   (((_target == null) && (_distanceToTarget > _minimumPathDistance)) ||
                    (_target != null) && (horizontalMovement > _minimumTargetDistance));
        }

        protected override void OnUpdatePath(Vector2 destination)
        {
            _currentSpeed = (_movementSpeed * Time.deltaTime);

            _movementPosition = Vector2.MoveTowards(_body.position, destination, _currentSpeed);
            _movementPosition.y = _body.position.y;
        }

        protected override void OnUpdateMovement()
        {
            base.OnUpdateMovement();

            _body.MovePosition(_movementPosition);
        }
    }
}