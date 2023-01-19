using Tenacity.Battles.Lands.Data;
using UnityEngine;


namespace Tenacity.Battles.Generators.Battles.Generators.Field
{
    public abstract class CellFabricSO : ScriptableObject
    {
        public abstract GameObject CreateCell(LandType type);
    }
}