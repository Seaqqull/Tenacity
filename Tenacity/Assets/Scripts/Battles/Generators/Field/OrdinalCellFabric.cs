using Tenacity.Battles.Lands.Data;
using System.Linq;
using UnityEngine;


namespace Tenacity.Battles.Generators.Battles.Generators.Field
{
    [System.Serializable]
    public class Cell
    {
        public LandType Type;
        public GameObject Land;
    }
    
    [CreateAssetMenu(fileName = "OrdinalCellFabric", menuName = "Battles/Field/OrdinalCellFabric")]
    public class OrdinalCellFabric : CellFabricSO
    {
        [SerializeField] private Cell[] _cells;
        
        public override GameObject CreateCell(LandType type)
        {
            var cellToSpawn = _cells.FirstOrDefault(cell => cell.Type.HasFlag(type));
            if (cellToSpawn == null)
            {
                Debug.LogError($"[OrdinalCellFabric] Error: required cell({type}) wasn't found.");
                return null;
            }
            return Instantiate(cellToSpawn.Land, Vector3.zero, Quaternion.identity);
        }
    }
}