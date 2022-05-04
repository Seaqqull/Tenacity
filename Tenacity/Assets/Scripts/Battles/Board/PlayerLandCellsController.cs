using System.Collections;
using System.Collections.Generic;
using Tenacity.Battles.Lands;
using UnityEngine;

namespace Tenacity.Battles.Controllers
{
    public class PlayerLandCellsController
    {
        private HashSet<Land> _availableLands = new HashSet<Land>();

        public HashSet<Land> AvailableLands => _availableLands;

        public void AddAvailableLand(Land land)
        {
            if (land == null) return;
            _availableLands.Add(land);
            _availableLands.UnionWith(land.NeighborLands);
        }

        public bool IsLandAvailable(Land land)
        {
            return _availableLands.Contains(land);
        }
    }
}