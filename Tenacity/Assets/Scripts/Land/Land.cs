using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tenacity.Lands
{
    public enum LandType
    {
        None = 0,
        Neutral = 1,
        Water = 2,
        Fire = 4,
        Earth = 8,
        Ice = 16
    }

    public class Land : MonoBehaviour
    {
        [SerializeField] private LandType type;
        [SerializeField] private bool isAvailable;

        public bool IsPlacedOnBoard
        {
            get => _isPlacedOnBoard;
            set => _isPlacedOnBoard = value;
        }
        public bool IsAvailable
        {
            get => isAvailable;
            set => isAvailable = (type != LandType.None) ? value : false;
        }

        public LandType Type => type;

        private bool _isPlacedOnBoard;

        //temp
        public List<Land> GetNeighborsList()
        {
            return null;
        }
    }
}
