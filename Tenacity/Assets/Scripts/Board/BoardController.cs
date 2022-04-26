using Tenacity.Lands;
using UnityEngine;

namespace Tenacity.Boards
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private Land _emptyLandPrefab;

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
                    CreateLandCell(row, col);
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

        private void CreateLandCell(float y, float x)
        {
            var landGO = Instantiate(
                _emptyLandPrefab.gameObject,
                new Vector3(x, 0, y),
                Quaternion.identity);
            landGO.transform.parent = transform;
            landGO.GetComponent<Land>().IsPlacedOnBoard = true;
            _board.AddCell(landGO.GetComponent<Land>(), x, y);
        }
    }
}