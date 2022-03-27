using System.Collections;
using UnityEngine;
using System;


namespace Tenacity.Behaviour
{
    public class Entity : Base.BaseMono, Utility.Interfaces.IIdentifiable, Utility.Interfaces.IRunLater, Utility.Interfaces.IDamageable
    {
        [field: SerializeField] public Health Health { get; protected set; }
        [SerializeField] protected Transform _gFX;
        [Header("Events")]
        [SerializeField] protected UnityEngine.Events.UnityEvent _onDead;
        
        protected bool _movementPossible;
        protected Rigidbody2D _body;
        
        public bool IsDead {get; protected set;}
        public int Id {get; private set;}
        
        
        protected override void Awake()
        {
            base.Awake();

            _body = GetComponent<Rigidbody2D>();
            Health = GetComponent<Health>();
            Id = GameObject.GetInstanceID();
            IsDead = false;
            
            if(Health != null)
            {
                Health.OnHealthMinus += OnHealthMinus;
                Health.OnHealthPlus += OnHealthPlus;
            }
            else
            {
                Health = Health.HealthNo.Instance;
            }
        }


        protected virtual void OnDeath()
        {
            _movementPossible = false;
            IsDead = true;

            _onDead.Invoke();
        }

        protected virtual bool CheckDead()
        {
            if (IsDead) return IsDead;
            return Health.IsZero;
        }

        protected virtual void OnHealthPlus()
        { }

        protected virtual void OnHealthMinus()
        {
            if (CheckDead())
                OnDeath();
        }


        public void PerformDamage(int amount)
        {
            Health.ModifyHealth(amount);
        }

        public virtual void ApplyForce(Vector3 direction, ForceMode2D force = ForceMode2D.Force)
        {
            _movementPossible = false;

            _body.AddForce(direction * _body.mass, force);
        }
        
        #region RunLater
        public void RunLater(Action method, float waitSeconds)
        {
            RunLaterValued(method, waitSeconds);
        }

        public Coroutine RunLaterValued(Action method, float waitSeconds)
        {
            if ((waitSeconds < 0) || (method == null))
                return null;

            return StartCoroutine(RunLaterCoroutine(method, waitSeconds));
        }

        public IEnumerator RunLaterCoroutine(Action method, float waitSeconds)
        {
            yield return new WaitForSeconds(waitSeconds);
            method();
        }        
        #endregion
    }
}