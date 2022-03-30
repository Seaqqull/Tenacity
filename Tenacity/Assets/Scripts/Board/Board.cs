using System.Collections;
using System.Collections.Generic;
using Tenacity.Lands;
using UnityEngine;

namespace Tenacity.Battle
{
    public class Board : MonoBehaviour
    {
        private List<Land> _landCells = new List<Land>();

        public List<Land> LandCells => _landCells;

        public void AddCell(Land land)
        {
            _landCells.Add(land);
        }
    }
}