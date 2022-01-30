using UnityEngine;
using System;


namespace Tenacity.Behaviour
{
    public abstract class Health : MonoBehaviour
    {
        public class HealthNo : Health
        {
            protected static Health _instance;

            public override int MaxValue
            {
                get { return 0; }
            }
            public override bool IsZero
            {
                get { return false; }
            }
            public override int Value
            {
                get { return 0; }
            }


            public static Health Instance
            {
                get
                {
                    if (_instance == null)
                    {// Later insert dummy objects inside of system -> dummy object (to keep scene clean)
                        GameObject instance = new GameObject("HealthNo", typeof(HealthNo));
                        instance.transform.SetAsFirstSibling();

                        _instance = instance.GetComponent<HealthNo>();
                    }

                    return _instance;
                }
            }


            public override void ResetHealth(int amount) { }

            public override void ModifyHealth(int amount) { }
        }


        protected Action _onHealthMinus;
        protected Action _onHealthPlus;

        public event Action OnHealthMinus
        {
            add { this._onHealthMinus += value; }
            remove { this._onHealthMinus -= value; }
        }
        public event Action OnHealthPlus
        {
            add { this._onHealthPlus += value; }
            remove { this._onHealthPlus -= value; }
        }

        public float Percent
        {
            get { return 1.0f * Value / MaxValue; }
        }
        public abstract int MaxValue { get; }
        public abstract bool IsZero { get; }
        public abstract int Value { get; }


        protected virtual void Awake() { }
        protected virtual void Start() { }

        private void OnDestroy()
        {
            _onHealthMinus = null;
            _onHealthPlus = null;
        }


        public virtual void ResetHealth()
        {
            ResetHealth(MaxValue);
        }

        public virtual void ModifyHealth(int amount)
        {
            if (amount > 0)
                _onHealthMinus.Invoke();
            else if (amount < 0)
                _onHealthPlus.Invoke();
        }


        public abstract void ResetHealth(int amount);
    }
}