using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tenacity.Battles.Lands;
using Tenacity.Cards;
using UnityEngine;

namespace Tenacity.Battles.Controllers
{
    public class HeroLandCellsController
    {
        private HashSet<Land> _availableLands = new HashSet<Land>();

        public List<Land> AvailableLands => _availableLands.ToList();
        public List<Land> FreeAvailableLands => AvailableLands.FindAll(land => !land.GetComponentInChildren<Card>()).ToList();
            
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