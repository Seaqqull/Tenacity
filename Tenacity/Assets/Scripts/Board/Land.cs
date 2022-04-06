using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tenacity.Lands
{
   
    [Flags] public enum LandType
    {
        None = 0,
        Ice = 1,
        Water = 2,
        Fire = 4,
        Earth = 8,
        Neutral = Ice | Water | Fire | Earth
    }

    public class Land : MonoBehaviour
    {
        [SerializeField] private LandType type;
        [SerializeField] private bool isAvailableForCards;
        [SerializeField] private bool isPlacedOnBoard;
        [SerializeField] private Material outliner;

        public LandType Type => type;
        public (float, float) CellId
        {
            get => _cellId;
            set => _cellId = value;
        }
        public bool IsPlacedOnBoard
        {
            get => isPlacedOnBoard;
            set => isPlacedOnBoard = value;
        }
        public bool IsAvailableForCards
        {
            get => isAvailableForCards;
            set => isAvailableForCards = (type != LandType.None) ? value : false;
        }

        private (float, float) _cellId;
        private List<Land> _neigborLands;
        private MeshRenderer _meshRenderer;
        private Material _standardMaterial;


        public List<Land> NeighborLands
        {
            get => _neigborLands;
            set => _neigborLands = value;
        }

        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        private GameObject LoadFromDatabase(Land newLandCard)
        {
            return AssetDatabase.LoadAssetAtPath(
                $"Assets/StaticAssets/Prefabs/Lands/land_{newLandCard.Type}.prefab", typeof(GameObject)
                ) as GameObject;
        }

        public bool ReplaceEmptyLand(Land newLandCard)
        {
            if (newLandCard == null || newLandCard.isAvailableForCards) return false;
            GameObject landGameObject = LoadFromDatabase(newLandCard);
            if (landGameObject == null) return false;

            MeshRenderer _meshRenderer = transform.GetComponentInChildren<MeshRenderer>();
            MeshRenderer newLandMeshRenderer = landGameObject.GetComponentInChildren<MeshRenderer>();
            _meshRenderer.sharedMaterials = newLandMeshRenderer.sharedMaterials;
            _standardMaterial = newLandMeshRenderer.sharedMaterial;

            type = landGameObject.GetComponent<Land>().Type;
            isPlacedOnBoard = true;
            IsAvailableForCards = true;
         
            if (GetComponent<LandController>() != null) GetComponent<LandController>().enabled = true;
            Destroy(newLandCard.gameObject);
            return true;
        }

        public void OutlineLand(bool outlined)
        {
            if (type == LandType.None) return;
            _meshRenderer.sharedMaterial = outlined ? outliner : _standardMaterial;
        }
    }
}
