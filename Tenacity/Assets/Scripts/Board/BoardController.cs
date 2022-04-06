using System.Collections;
using System.Collections.Generic;
using Tenacity.Lands;
using UnityEngine;
using EngineInput = UnityEngine.Input;

namespace Tenacity.Battle
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private Board board;
        [SerializeField] private Land emptyLandPrefab;

        private void Start()
        {
            if (board == null) return;
            if (emptyLandPrefab == null || !emptyLandPrefab.GetComponent<Land>()) return;
            InitBoard();
        }

        private void InitBoard()
        {
            CreateCells();
            SetNeighbors();
        }

        private void CreateCells()
        {
            int size = board.MapRadius;
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
            foreach (Land land in board.LandCells)
            {
                land.NeighborLands = board.GetCellNeighbors(land);
            }
        }

        private void CreateLandCell(float y, float x)
        {
            var landGO = Instantiate(
                emptyLandPrefab.gameObject,
                new Vector3(x, 0, y),
                Quaternion.identity);
            landGO.transform.parent = transform;
            landGO.GetComponent<Land>().IsPlacedOnBoard = true;
            board.AddCell(landGO.GetComponent<Land>(), x, y);
        }
    }
}