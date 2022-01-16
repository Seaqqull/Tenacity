using UnityEngine;
using System;


namespace Tenacity.Properties
{
    [Serializable]
    public class IntegerReference
    {
        [SerializeField] private bool UseConstant = true;
        [SerializeField] private int ConstantValue;
#pragma warning disable 0649
        [SerializeField] private IntegerVariable Variable;
#pragma warning restore 0649

        public int Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }


        public IntegerReference()
        {
        }

        public IntegerReference(int value)
        {
            UseConstant = true;
            ConstantValue = value;
        }


        public static implicit operator int(IntegerReference reference)
        {
            return reference.Value;
        }
    }
}