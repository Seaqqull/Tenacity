using Tenacity.Battles.Lands.Data;
using System.Collections.Generic;
using Tenacity.Cards;
using UnityEngine;


namespace Tenacity.Battles.Lands
{
    public class Land : MonoBehaviour
    {
        [SerializeField] private LandType _type;
        [SerializeField] private bool _isAvailableForCards;
        [SerializeField] private bool _isPlacedOnBoard;
        [SerializeField] private Material outliner;
        [SerializeField] private float _topPoint = 0.61f;

        private MeshRenderer _meshRenderer;
        private Material _standardMaterial;
        private List<Land> _neigborLands;
        private int _cellId;

        public float TopPoint => _topPoint;
        public List<Land> NeighborLands
        {
            get => _neigborLands;
            set => _neigborLands = value;
        }
        public bool IsStartPosition
        {
            get; set;
        }
        public bool IsAvailableForCards
        {
            get => (_type != LandType.None && GetComponentInChildren<Card>() == null && !IsStartPosition);
        }
        public LandType Type => _type;
        public bool IsPlacedOnBoard
        {
            get => _isPlacedOnBoard;
            set => _isPlacedOnBoard = value;
        }
        public int CellId
        {
            get => _cellId;
            set => _cellId = value;
        }
        public Vector2 CellPosition
        {
            get; set;
        }


        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        
        private GameObject LoadFromDatabase(Land newLandCard)
        {
            return Resources.Load<GameObject>($"Lands/land_{newLandCard.Type}");
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
        
        public bool ReplaceLand(Land newLandCard, bool isEnemy=false)
        {
            if ((newLandCard == null) || (newLandCard._isAvailableForCards)) return false;
            GameObject landGameObject = LoadFromDatabase(newLandCard);
            if (landGameObject == null) return false;

            MeshRenderer _meshRenderer = transform.GetComponentInChildren<MeshRenderer>();
            MeshRenderer newLandMeshRenderer = landGameObject.GetComponentInChildren<MeshRenderer>();
             _meshRenderer.sharedMaterials = newLandMeshRenderer.sharedMaterials;
            _meshRenderer.material.SetFloat("_Metallic", isEnemy ? 0.5f : 0);
            _standardMaterial = _meshRenderer.material;

            _type = landGameObject.GetComponent<Land>().Type;
            _isPlacedOnBoard = true;

            if (TryGetComponent<LandCellController>(out var landController)) 
                landController.enabled = true;
            return true;
        }

        public bool NeighborListContains(Land land)
        {
            return NeighborLands.Contains(land);
        }
    }
}
