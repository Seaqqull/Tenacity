using UnityEngine;


namespace Tenacity.Properties
{
    [CreateAssetMenu(menuName = "Variable/Integer")]
    public class IntegerVariable : ScriptableObject
    {
#if UNITY_EDITOR
#pragma warning disable 0414
        [Multiline] [SerializeField] private string _description = "";
#pragma warning restore 0414
#endif
        [SerializeField] private int _value;

        public int Value
        {
            get { return this._value; }
            private set { this._value = value; }
        }


        public void SetValue(int value)
        {
            Value = value;
        }

        public void SetValue(IntegerVariable value)
        {
            Value = value.Value;
        }


        public void ApplyChange(int amount)
        {
            Value += amount;
        }

        public void ApplyChange(IntegerVariable amount)
        {
            Value += amount.Value;
        }
    }
}