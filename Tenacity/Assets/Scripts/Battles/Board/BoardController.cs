using Tenacity.Battles.Lands;
using UnityEngine;
using TMPro;


namespace Tenacity.Battles.Boards
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private Land _emptyLandPrefab;
        [SerializeField] private TextMeshPro _textField;

        private Board _board;
        private int id;


        private void Awake()
        {
            _board = GetComponent<Board>();
        }

        private void Start()
        {
            if (_board == null || _emptyLandPrefab == null || !_emptyLandPrefab.GetComponent<Land>()) return;
            InitBoard();
        }

        
        private void InitBoard()
        {
            CreateCells();
            SetNeighbors();
        }

        private void CreateCells()
        {
            int size = _board.MapRadius;
            float colLimit = 2 * (size - 1);
            
            for (float row = -size + 1; row < size; row++)
                for (float col = -(colLimit - Mathf.Abs(row)) / 2; col <= (colLimit - Mathf.Abs(row)) / 2; col++)
                    CreateLandCell(row, col);
            InitStartPositions();
        }

        private void InitStartPositions()
        {
            _board.LandCells[0].IsStartPosition = true;
            _board.LandCells[^1].IsStartPosition = true;
        }

        private void SetNeighbors()
        {
            foreach (Land land in _board.LandCells)
                land.NeighborLands = _board.GetCellNeighbors(land);
        }

        private void tmpCellCounter(GameObject landGO)
        {
            var textObj = Instantiate(_textField);
            var text = textObj.GetComponent<TextMeshPro>();
            var textTransform = textObj.transform;
            
            textTransform.SetParent(landGO.transform);
            textTransform.localPosition = new Vector3(0, 0.4f, 0);
            
            text.text = (id++).ToString();
        }
        
        private void CreateLandCell(float y, float x)
        {
            var landGO = Instantiate(_emptyLandPrefab.gameObject, new Vector3(x, 0, y), Quaternion.identity);
            landGO.transform.parent = transform;

            var land = landGO.GetComponent<Land>();

            _board.AddCell(land, x, y);

            tmpCellCounter(landGO);    
        }
        
    }
}