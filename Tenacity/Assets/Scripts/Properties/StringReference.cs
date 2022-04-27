using UnityEngine;
using System;


namespace Tenacity.Properties
{
    [Serializable]
    public class StringReference
    {
#pragma warning disable 0649
        [SerializeField] private bool UseConstant = true;
        [SerializeField] private string ConstantValue;
        [SerializeField] private StringVariable Variable;
#pragma warning restore 0649

        public string Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }


        public StringReference()
        {
        }

        public StringReference(string value)
        {
            UseConstant = true;
            ConstantValue = value;
        }


        public static implicit operator string(StringReference reference)
        {
            return reference.Value;
        }
    }
}