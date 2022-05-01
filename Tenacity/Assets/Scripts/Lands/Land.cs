using System;
using System.Collections.Generic;
using Tenacity.Cards;
using UnityEditor;
using UnityEngine;

namespace Tenacity.Lands
{
   
    [Flags] public enum LandType
    {
        None = 0,
        Ice = 1 << 0,
        Water = 1 << 1,
        Fire = 1 << 2,
        Earth = 1 << 3,
        Neutral = ~0,
    }

    public class Land : MonoBehaviour
    {
        [SerializeField] private LandType _type;
        [SerializeField] private bool _isAvailableForCards;
        [SerializeField] private bool _isPlacedOnBoard;
        [SerializeField] private Material outliner;
        [SerializeField] private float _topPoint = 0.61f;

        private int _cellId;
        private List<Land> _neigborLands;
        private MeshRenderer _meshRenderer;
        private Material _standardMaterial;

        public List<Land> NeighborLands
        {
            get => _neigborLands;
            set => _neigborLands = value;
        }


        public LandType Type => _type;
        public float TopPoint => _topPoint;

        public int CellId
        {
            get => _cellId;
            set => _cellId = value;
        }
        public bool IsPlacedOnBoard
        {
            get => _isPlacedOnBoard;
            set => _isPlacedOnBoard = value;
        }
        public bool IsAvailableForCards
        {
            get => (_type != LandType.None && GetComponentInChildren<Card>() == null);
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


        public Card GetPlacedCreature()
        {
            return transform.GetComponentInChildren<Card>();
        }
       
        public void OutlineLand(bool outlined)
        {
            if (_type == LandType.None) return;
            _meshRenderer.sharedMaterial = outlined ? outliner : _standardMaterial;
        }
        
        public bool ReplaceLand(Land newLandCard)
        {
            if (newLandCard == null || newLandCard._isAvailableForCards) return false;
            GameObject landGameObject = LoadFromDatabase(newLandCard);
            if (landGameObject == null) return false;

            MeshRenderer _meshRenderer = transform.GetComponentInChildren<MeshRenderer>();
            MeshRenderer newLandMeshRenderer = landGameObject.GetComponentInChildren<MeshRenderer>();
            _meshRenderer.sharedMaterials = newLandMeshRenderer.sharedMaterials;
            _standardMaterial = newLandMeshRenderer.sharedMaterial;

            _type = landGameObject.GetComponent<Land>().Type;
            _isPlacedOnBoard = true;

            if (GetComponent<LandCellController>() != null) GetComponent<LandCellController>().enabled = true;

            return true;
        }

        public bool NeighborListContains(Land land)
        {
            return NeighborLands.Contains(land);
        }

    }
}
