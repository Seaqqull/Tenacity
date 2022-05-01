using Tenacity.Lands;
using TMPro;
using UnityEngine;

namespace Tenacity.Boards
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
            if (_board == null) return;
            if (_emptyLandPrefab == null || !_emptyLandPrefab.GetComponent<Land>()) return;
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
            {
                for (float col = -(colLimit - Mathf.Abs(row)) / 2; col <= (colLimit - Mathf.Abs(row)) / 2; col++)
                {
                    bool isAvailable = true;
                    if (row == -size + 1 && col == -(colLimit - Mathf.Abs(row)) / 2) isAvailable = false;
                    if (row == size - 1 && col == (colLimit - Mathf.Abs(row)) / 2) isAvailable = false;
                    CreateLandCell(row, col, isAvailable);
                }
            }
        }

        private void SetNeighbors()
        {
            foreach (Land land in _board.LandCells)
            {
                land.NeighborLands = _board.GetCellNeighbors(land);
            }
        }

        private void CreateLandCell(float y, float x, bool isAvailable)
        {
            var landGO = Instantiate(_emptyLandPrefab.gameObject, new Vector3(x, 0, y), Quaternion.identity);
            landGO.transform.parent = transform;

            Land land = landGO.GetComponent<Land>();

            if (isAvailable) _board.AddCell(land, x, y);
            else
            {
                land.GetComponent<LandCellController>().enabled = false;
                _board.AddStartPosition(land);
            } 

            tmpCellCounter(landGO);    
        }

        private void tmpCellCounter(GameObject landGO)
        {
            var textObj = Instantiate(_textField);
            var text = textObj.GetComponent<TextMeshPro>();
            textObj.transform.SetParent(landGO.transform);
            textObj.transform.localPosition = new Vector3(0, 0.4f, 0);
            text.text = (id++).ToString();
        }
    }
}