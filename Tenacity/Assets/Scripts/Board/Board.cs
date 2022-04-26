using System.Collections.Generic;
using System.Linq;
using Tenacity.Lands;
using TMPro;
using UnityEngine;

namespace Tenacity.Boards
{
    [CreateAssetMenu(fileName = "Board Template", menuName = "Board")]
    public class Board : ScriptableObject
    {
        [SerializeField] private int _mapRadius;

        public int MapRadius => _mapRadius;
        public List<Land> LandCells => _landsMap.Values.ToList();

        private List<Land> _landCells = new List<Land>();
        private Dictionary<(float, float), Land> _landsMap = new Dictionary<(float, float), Land>();

        private readonly float[,] _neighborsMatrix = new float[6, 2]
        {
           { 0, -1},
           { 1, -0.5f},
           { 1, 0.5f},
           { 0, 1},
           { -1, 0.5f},
           { -1, -0.5f}
        };


        public void AddCell(Land land, float x, float y)
        {
            land.CellId = (y, x);
            _landsMap.Add((y, x), land);
            _landCells.Add(land);
        }
        
        public List<Land> GetCellNeighbors(Land cell)
        {
            List<Land> neighbors = new List<Land>();
            for (int i = 0; i < _neighborsMatrix.GetLength(0); i++)
            {
                float rowPos = cell.CellId.Item1 + _neighborsMatrix[i, 0];
                float colPos = cell.CellId.Item2 + _neighborsMatrix[i, 1];
                Land neighborLand = _landsMap.GetValueOrDefault((rowPos, colPos));
                if (neighborLand != null)
                    neighbors.Add(neighborLand);
            }
            return neighbors;
        }
    }
}