using UnityEngine;


namespace Tenacity.Properties
{
    [CreateAssetMenu(menuName = "Variable/String")]
    public class StringVariable : ScriptableObject
    {

#if UNITY_EDITOR
#pragma warning disable 0414
        [Multiline] [SerializeField] private string _description = "";
#pragma warning restore 0414
#endif
        [SerializeField] private string _value;


        public string Value
        {
            get { return this._value; }
            private set { this._value = value; }
        }


        public void SetValue(string value)
        {
            Value = value;
        }

        public void SetValue(StringVariable value)
        {
            Value = value.Value;
        }
    }
}