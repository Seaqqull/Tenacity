using Tenacity.Behaviour.Enemies.Data;
using System.Collections.Generic;
using UnityEngine;


namespace Tenacity.General.Events.Triggers
{
    public class TargetSpectator : MonoBehaviour
    {
        [SerializeField] private List<Behaviour.Enemies.Enemy> _subscribers;
        [SerializeField] private Behaviour.Entity _target;

        private Collider2D _collider;


        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if(_target == null) return;
            if (_target.IsDead)
            {
                _target = null;
                for (int i = 0; i < _subscribers.Count; i++)
                    _subscribers[i].Target = null;
                return;
            }

            ViewDirection detectionSide;
            float targetDistance;

            for(int i = 0; i < _subscribers.Count; i++)
            {
                detectionSide = (_target.Position.x > _subscribers[i].Position.x) ? ViewDirection.Right :
                    (_target.Position.x < _subscribers[i].Position.x) ?  ViewDirection.Left :  ViewDirection.Center;
                targetDistance = Vector2.SqrMagnitude((_target.Position - _subscribers[i].Position));

                if((!_subscribers[i].DirectDetectionOnly && targetDistance <= _subscribers[i].DetectionDistance.y) ||
                   (_subscribers[i].Spectation.HasFlag(detectionSide) && targetDistance <= _subscribers[i].DetectionDistance.y) || // Direct view
                   (!_subscribers[i].Spectation.HasFlag(detectionSide) && targetDistance <= _subscribers[i].DetectionDistance.x))  // Indirect view
                {
                    if(_subscribers[i].Target == null)
                        _subscribers[i].Target = _target.Transform;
                }
                else
                {
                    _subscribers[i].Target = null;
                }
            }
        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            var intruder = collision.gameObject;

            if(intruder.TryGetComponent<Behaviour.Enemies.Enemy>(out var enemy))
            {
                _subscribers.Add(enemy);
            }
            else if(intruder.TryGetComponent<Behaviour.Entity>(out var target)) // Here dedicated Player class
            {
                _target = target;
            }
        }

        public virtual void OnTriggerExit2D(Collider2D collision)
        {
            var leaver = collision.gameObject;

            if(leaver.TryGetComponent<Behaviour.Enemies.Enemy>(out var enemy))
            {
                _subscribers.Remove(enemy);
            }
            else if(leaver.TryGetComponent<Behaviour.Entity>(out var target)) // Here dedicated Player class
            {
                _target = null;
            }
        }
    }
}
