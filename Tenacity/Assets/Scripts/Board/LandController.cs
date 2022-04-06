using System.Collections;
using System.Collections.Generic;
using Tenacity.Battle;
using UnityEngine;

namespace Tenacity.Lands
{
    public class LandController : MonoBehaviour
    {
        private Land _land;
        private Board _board;

        private void Start()
        {
            _board = transform.parent.GetComponent<Board>();
            _land = GetComponent<Land>();
        }

        public void HighlightNeighbors(LandType selectedType, bool highlighted)
        {
            if (!enabled) return;
            
            _land.OutlineLand(highlighted);
            foreach (Land neighbor in _land.NeighborLands)
            {
                if (neighbor.Type.HasFlag(selectedType))
                    neighbor.OutlineLand(highlighted);
            }
        }

    }
}