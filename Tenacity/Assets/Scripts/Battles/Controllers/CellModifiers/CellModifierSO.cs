using UnityEngine;


namespace Tenacity.Battles.Controllers.CellModifiers
{
    public abstract class CellModifierSO : ScriptableObject
    {
        public abstract void ModifyCell(CellController cell);
    }
}