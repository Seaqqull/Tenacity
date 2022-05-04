using System.Collections.Generic;
using Tenacity.Battles.Lands;
using UnityEngine;
using System.Linq;


namespace Tenacity.Battles.Boards
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private int _mapRadius;
        
        private Dictionary<Vector2, Land> _landsMap = new Dictionary<Vector2, Land>();
        private readonly float[,] _neighborsMatrix = new float[6, 2]
        {
           { 0, -1},
           { 1, -0.5f},
           { 1, 0.5f},
           { 0, 1},
           { -1, 0.5f},
           { -1, -0.5f}
        };
        private List<Land> _startPositions = new List<Land>();
        
        public List<Land> LandCells => _landsMap.Values.ToList();
        public int MapRadius => _mapRadius;

        
        public List<Land> GetCellNeighbors(Land cell)
        {
            var neighbors = new List<Land>();
            for (int i = 0; i < _neighborsMatrix.GetLength(0); i++)
            {
                Vector2 cellPosition = _landsMap.FirstOrDefault(el => el.Value == cell).Key;
                float rowPos = cellPosition.y + _neighborsMatrix[i, 0];
                float colPos = cellPosition.x + _neighborsMatrix[i, 1];
                Vector2 neighborKey = new Vector2(colPos, rowPos);
                Land neighborLand = _landsMap.GetValueOrDefault(neighborKey);
                if (neighborLand != null)
                    neighbors.Add(neighborLand);
            }
            return neighbors;
        }
        
        public void AddCell(Land land, float x, float y)
        {
            land.CellId = _landsMap.Count;
            _landsMap.Add(new Vector2(x, y), land);
        }
    }
}