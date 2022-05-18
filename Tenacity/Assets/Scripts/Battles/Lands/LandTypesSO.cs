using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tenacity.Battles.Lands
{
    [CreateAssetMenu(fileName = "LandTypes Template", menuName = "Land/LandTypes")]
    public class LandTypesSO : ScriptableObject
    {
        [SerializeField] private List<Land> _landReferences;

        public List<Land> LandReferences => _landReferences;
    }
}