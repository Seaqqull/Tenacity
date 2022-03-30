using System.Collections.Generic;
using Tenacity.Battle;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private bool isAvailableForCards;
        [SerializeField] private bool isPlacedOnBoard;

        private List<Land> _neigborLands;
        
        public bool IsAvailableForCards
        {
            get => isAvailableForCards;
            set => isAvailableForCards = (type != LandType.None) ? value : false;
        }
        public bool IsPlacedOnBoard
        {
            get => isPlacedOnBoard;
            set => isPlacedOnBoard = value;
        }
        public LandType Type => type;
        public List<Land> NeighborLands
        {
            get => _neigborLands;
            set => _neigborLands = value;
        }


        public bool ReplaceEmptyLand(Land newLandCard)
        {
            if (newLandCard == null || newLandCard.isAvailableForCards) return false;

            GameObject landGameObject = LoadFromDatabase(newLandCard);
            if (landGameObject == null) return false;

            var currentLandMeshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
            var newLandMeshRenderer = landGameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
            currentLandMeshRenderer.sharedMaterials = newLandMeshRenderer.sharedMaterials;
            
            type = landGameObject.GetComponent<Land>().Type;
            isPlacedOnBoard = true;
            IsAvailableForCards = true;

            Destroy(newLandCard.gameObject);
            return true;
        }

        private GameObject LoadFromDatabase(Land newLandCard)
        {
            return AssetDatabase.LoadAssetAtPath(
                $"Assets/StaticAssets/Prefabs/Lands/land_{newLandCard.Type}.prefab", typeof(GameObject)
                ) as GameObject;
        }
    }
}
